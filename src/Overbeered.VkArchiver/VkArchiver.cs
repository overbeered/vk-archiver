using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Overbeered.VkArchiver.Converters;
using System.IO.Compression;
using VkNet;
using VkNet.AudioBypassService.Extensions;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace Overbeered.VkArchiver
{
    public class VkArchiver : IVkArchiver
    {
        private readonly VkApi _vkApi;

        /// <summary>
        /// E-mail или телефон
        /// </summary>
        public string Login { get; private set; }

        /// <summary>
        /// Пароль для авторизации
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// ID приложения
        /// </summary>
        public ulong ApplicationId { get; private set; }

        /// <summary>
        /// Логгер
        /// </summary>
        private readonly ILogger<VkArchiver> _logger;

        public VkArchiver(string login, string password) : this(login, password, NullLogger<VkArchiver>.Instance)
        {

        }

        public VkArchiver(string login, string password, ILogger<VkArchiver> logger)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json").Build();
            var application = config.GetSection(nameof(Application)).Get<Application>();

            Login = login;
            Password = password;
            ApplicationId = application.Id;
            _vkApi = new VkApi(new ServiceCollection().AddAudioBypass());
            _logger = logger;
            try
            {
                _vkApi.Authorize(new ApiAuthParams()
                {
                    ApplicationId = ApplicationId,
                    Login = Login,
                    Password = Password,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in VkArchiver");
            }
        }

        public async Task ArchiveAsync(string path, FromPeer fromPeer = FromPeer.All, FromMedia fromMedia = FromMedia.Photo)
        {
            // Максимум в ВК можно сделать один запрос на 200 последних бесед
            ulong? offset = 0;
            long? count;
            try
            {
                do
                {
                    var conversations = _vkApi.Messages.GetConversations(new GetConversationsParams()
                    {
                        Offset = offset,
                        Count = 200,
                    });
                    offset += 200;
                    count = conversations.Items.Count;

                    foreach (var Item in conversations.Items)
                    {
                        string pathName = String.Empty;

                        if (Item.Conversation.Peer.Type.ToString() == "chat" && (FromPeer.All == fromPeer || FromPeer.Chats == fromPeer))
                        {
                            pathName = string.Concat(path, @"\", Item.Conversation.ChatSettings.Title);
                        }

                        if (Item.Conversation.Peer.Type.ToString() == "user" && (FromPeer.All == fromPeer || FromPeer.Dialogs == fromPeer))
                        {
                            var user = _vkApi.Users.Get(new long[] { Item.Conversation.Peer.Id });
                            var userName = user[0].FirstName != "DELETED" ? $"{user[0].FirstName} {user[0].LastName}" : $"{Item.Conversation.Peer.Id}";
                            pathName = string.Concat(path, @"\", userName);
                        }

                        if (!string.IsNullOrEmpty(pathName)) await DownloadArchiveFileAsync(Item.Conversation.Peer.Id, pathName, fromMedia);
                    }
                }
                while (count >= 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in VkArchiver");
            }
        }

        public async Task ArchiveAsync(string path, string name, FromMedia fromMedia = FromMedia.Photo)
        {
            // Максимум в ВК можно сделать один запрос на 200 последних бесед
            ulong? offset = 0;
            long? count;
            try
            {
                do
                {
                    var conversations = _vkApi.Messages.GetConversations(new GetConversationsParams()
                    {
                        Offset = offset,
                        Count = 200,
                    });
                    offset += 200;
                    count = conversations.Items.Count;

                    foreach (var Item in conversations.Items)
                    {
                        if (Item.Conversation.Peer.Type.ToString() == "chat" && Item.Conversation.ChatSettings.Title == name)
                        {
                            await DownloadArchiveFileAsync(Item.Conversation.Peer.Id,
                                string.Concat(path, @"\", Item.Conversation.ChatSettings.Title),
                                fromMedia);
                            break;
                        }

                        if (Item.Conversation.Peer.Type.ToString() == "user")
                        {
                            var user = _vkApi.Users.Get(new long[] { Item.Conversation.Peer.Id });
                            var userName = user[0].FirstName != "DELETED" ? $"{user[0].FirstName} {user[0].LastName}" : $"{Item.Conversation.Peer.Id}";

                            if ($"{user[0].FirstName} {user[0].LastName}" == name)
                            {
                                await DownloadArchiveFileAsync(Item.Conversation.Peer.Id,
                                    string.Concat(path, @"\", userName),
                                    fromMedia);
                                break;
                            }
                        }
                    }
                }
                while (count >= 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in VkArchiver");
            }
        }

        /// <summary>
        /// Скачивает все фотографии из чата/диалога
        /// </summary>
        /// <param name="id">Индификатор чата/диалога</param>
        /// <param name="path">Путь для сохранения фотографий</param>
        /// <param name="fromMedia">Тип медиа</param>
        private async Task DownloadArchiveFileAsync(long id, string path, FromMedia fromMedia)
        {
            try
            {
                // Максимум в ВК можно сделать один запрос на 200 материалов диалога или беседы
                string next = string.Empty;
                Directory.CreateDirectory(path);
                do
                {
                    var messagesHistory = _vkApi.Messages.GetHistoryAttachments(new MessagesGetHistoryAttachmentsParams()
                    {
                        PeerId = id,
                        MediaType = MediaTypeConverter.Converter(fromMedia),
                        Count = 200,
                        StartFrom = next,
                    }, out next);

                    foreach (var history in messagesHistory)
                    {
                        var uriFile = UriDownloadFile(fromMedia, history);
                        if (uriFile != null) await DownloadFileAsync(uriFile.Uri!, string.Concat(path, @"\", uriFile.Name));
                    }
                }
                while (!string.IsNullOrEmpty(next));

                ZipFile.CreateFromDirectory(path, string.Concat(path.Remove(path.Length), ".zip"));
                Directory.Delete(path, true);
            }
            // -2147024773 Неверный синтаксис имени файла, имени каталога или метки тома
            // -2147024893 Системе не удается найти указанный путь
            catch (Exception ex) when (ex.HResult == -2147024773 || ex.HResult == -2147024893)
            {
                await DownloadArchiveFileAsync(id, Filter.Directories(path), fromMedia);
            }
            // -2147024816 Файл существует
            catch (Exception ex) when (ex.HResult == -2147024816)
            {
                ZipFile.CreateFromDirectory(path, string.Concat(path.Remove(path.Length), "_", id, ".zip"));
                Directory.Delete(path, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in VkArchiver");
            }
        }

        /// <summary>
        /// URI и имя скачанного файла
        /// </summary>
        /// <param name="fromMedia">Тип медиа</param>
        /// <param name="historyAttachment">История файлов</param>
        /// <returns>URI и имя скачанного файла</returns>
        private static UriFile UriDownloadFile(FromMedia fromMedia, HistoryAttachment historyAttachment)
        {
            switch (fromMedia)
            {
                case FromMedia.Photo:
                    var photo = (Photo)historyAttachment.Attachment.Instance;
                    return new UriFile(photo.Sizes[^1].Url, photo.Sizes[^1].Url.Segments[^1]);
                case FromMedia.Doc:
                    var doc = (Document)historyAttachment.Attachment.Instance;
                    return new UriFile(new Uri(doc.Uri), doc.Title);
                default:
                    return new UriFile(null, string.Empty);
            };
        }

        /// <summary>
        /// Скачивание файла
        /// </summary>
        /// <param name="uri">Ссылка на скачивание файла</param>
        /// <param name="pathFile">Путь для сохранения файла с именем и расширением</param>
        private static async Task DownloadFileAsync(Uri uri, string pathFile)
        {
            using var httpClient = new HttpClient();
            using var stream = await httpClient.GetStreamAsync(uri);
            if (stream != null)
            {
                using var fileStream = new FileStream(pathFile, FileMode.OpenOrCreate);
                await stream.CopyToAsync(fileStream);
            }
        }
    }
}

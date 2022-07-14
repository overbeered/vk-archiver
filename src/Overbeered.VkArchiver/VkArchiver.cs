using Microsoft.Extensions.DependencyInjection;
using System.IO.Compression;
using VkNet;
using VkNet.AudioBypassService.Extensions;
using VkNet.Enums.SafetyEnums;
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
        ///  Путь до папки, куда будут складываться архивы
        /// </summary>
        public string BasePath { get; private set; }

        public VkArchiver(string login, string password, ulong applicationId, string basePath)
        {
            Login = login;
            Password = password;
            ApplicationId = applicationId;
            BasePath = basePath;
            _vkApi = new VkApi(new ServiceCollection().AddAudioBypass());

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
                DebugLog("Error in {VkArchiver}:\n" + ex.Message, nameof(VkArchiver));
            }
        }

        public async Task ArchivePhotosAsync(From from)
        {
            switch (from)
            {
                case From.All:
                    await ArchiveAllPhotosAsync();
                    break;
                case From.Dialogs:
                    await ArchiveDialogsPhotosAsync();
                    break;
                case From.Chats:
                    await ArchiveChatsPhotosAsync();
                    break;
            }
        }

        public async Task ArchivePhotosAsync(string name)
        {
            try
            {
                ulong? offset = 0;
                long? count;
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
                            var path = CreateDirectory(Item.Conversation.ChatSettings.Title);
                            await PeerPhotoDownloadAsync(Item.Conversation.Peer.Id, path);
                            CreateZipDirectory(path);
                            goto LoopEnd;
                        }

                        if (Item.Conversation.Peer.Type.ToString() == "user")
                        {
                            var user = _vkApi.Users.Get(new long[] { Item.Conversation.Peer.Id });
                            var userName = user[0].FirstName != "DELETED" ? $"{user[0].FirstName} {user[0].LastName}" : $"{Item.Conversation.Peer.Id}";

                            if ($"{user[0].FirstName} {user[0].LastName}" == name)
                            {
                                var path = CreateDirectory(userName);
                                await PeerPhotoDownloadAsync(Item.Conversation.Peer.Id, path);
                                CreateZipDirectory(path);
                                goto LoopEnd;
                            }
                        }

                    }
                }
                while (count >= 200);

            LoopEnd:;
            }
            catch(Exception ex)
            {
                DebugLog("Error in {VkArchiver}:\n" + ex.Message, nameof(VkArchiver));
            }
        }

        /// <summary>
        /// Aрхивирование всех фотографий из всех чатов и диалогов
        /// </summary>
        private async Task ArchiveAllPhotosAsync()
        {
            try
            {
                ulong? offset = 0;
                long? count;
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
                        if (Item.Conversation.Peer.Type.ToString() == "chat")
                        {
                            var path = CreateDirectory(Item.Conversation.ChatSettings.Title);
                            await PeerPhotoDownloadAsync(Item.Conversation.Peer.Id, path);
                            CreateZipDirectory(path);
                        }

                        if (Item.Conversation.Peer.Type.ToString() == "user")
                        {
                            var user = _vkApi.Users.Get(new long[] { Item.Conversation.Peer.Id });
                            var userName = user[0].FirstName != "DELETED" ? $"{user[0].FirstName} {user[0].LastName}" : $"{Item.Conversation.Peer.Id}";
                            var path = CreateDirectory(userName);
                            await PeerPhotoDownloadAsync(Item.Conversation.Peer.Id, path);
                            CreateZipDirectory(path);
                        }

                    }
                }
                while (count >= 200);
            }
            catch (Exception ex)
            {
                DebugLog("Error in {VkArchiver}:\n" + ex.Message, nameof(VkArchiver));
            }
        }

        /// <summary>
        /// Aрхивирование всех фотографий из всех диалогов
        /// </summary>
        private async Task ArchiveDialogsPhotosAsync()
        {
            try
            {
                ulong? offset = 0;
                long? count;
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
                        if (Item.Conversation.Peer.Type.ToString() == "user")
                        {
                            var user = _vkApi.Users.Get(new long[] { Item.Conversation.Peer.Id });
                            var userName = user[0].FirstName != "DELETED" ? $"{user[0].FirstName} {user[0].LastName}" : $"{Item.Conversation.Peer.Id}";
                            var path = CreateDirectory(userName);
                            await PeerPhotoDownloadAsync(Item.Conversation.Peer.Id, path);
                            CreateZipDirectory(path);
                        }

                    }
                }
                while (count >= 200);
            }
            catch (Exception ex)
            {
                DebugLog("Error in {VkArchiver}:\n" + ex.Message, nameof(VkArchiver));
            }
        }

        /// <summary>
        /// Aрхивирование всех фотографий из всех чатов
        /// </summary>
        private async Task ArchiveChatsPhotosAsync()
        {
            try
            {
                ulong? offset = 0;
                long? count;
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
                        if (Item.Conversation.Peer.Type.ToString() == "chat")
                        {
                            var path = CreateDirectory(Item.Conversation.ChatSettings.Title);
                            await PeerPhotoDownloadAsync(Item.Conversation.Peer.Id, path);
                            CreateZipDirectory(path);
                        }
                    }
                }
                while (count >= 200);
            }
            catch (Exception ex)
            {
                DebugLog("Error in {VkArchiver}:\n" + ex.Message, nameof(VkArchiver));
            }
        }

        /// <summary>
        /// Создает временную директорию от BasePath
        /// </summary>
        /// <param name="name">Название директории</param>
        /// <returns>Возвращает путь к созданной директории</returns>
        private string CreateDirectory(string name)
        {
            string path = BasePath + @"\" + name;
            try
            {
                Directory.CreateDirectory(path);
            } 
            catch (Exception ex) 
            {
                DebugLog("Error in {VkArchiver}:\n" + ex.Message, nameof(VkArchiver));
            }
            return path;
        }

        /// <summary>
        /// Удаляет временную директорию и создает архив 
        /// </summary>
        /// <param name="path">Путь к временной директории</param>
        private void CreateZipDirectory(string path)
        {
            try
            {
                ZipFile.CreateFromDirectory(path, path.Remove(path.Length - 1) + ".zip");
                Directory.Delete(path, true);
            }
            catch (Exception ex)
            {
                DebugLog("Error in {VkArchiver}:\n" + ex.Message, nameof(VkArchiver));
            }
        }

        /// <summary>
        /// Скачивает все фотографии из чата/диалога
        /// </summary>
        /// <param name="id">Индификатор чата/диалога</param>
        /// <param name="path">Путь к временной директории</param>
        private async Task PeerPhotoDownloadAsync(long id, string path)
        {
            string next = string.Empty;
            try
            {
                do
                {
                    var messagesHistory = _vkApi.Messages.GetHistoryAttachments(new MessagesGetHistoryAttachmentsParams()
                    {
                        PeerId = id,
                        MediaType = MediaType.Photo,
                        Count = 200,
                        StartFrom = next,
                    }, out next);

                    foreach (var history in messagesHistory)
                    {
                        var photo = (Photo)history.Attachment.Instance;

                        await DownloadAsync(path
                            + photo.Id
                            + photo.Sizes[photo.Sizes.Count - 1].Url.LocalPath.Remove(0, photo.Sizes[photo.Sizes.Count - 1].Url.LocalPath.IndexOf(".")),
                            photo.Sizes[photo.Sizes.Count - 1].Url.OriginalString);
                    }
                }
                while (next != null);
            }
            catch (Exception ex)
            {
                DebugLog("Error in {VkArchiver}:\n" + ex.Message, nameof(VkArchiver));
            }
        }

        /// <summary>
        /// Скачивает файл
        /// </summary>
        /// <param name="path">Путь загрузки директории с названием и расширением</param>
        /// <param name="url">Ссылка для загрузки</param>
        private async Task DownloadAsync(string path, string url)
        {
            try
            {
                using var client = new HttpClient();
                using var stream = await client.GetStreamAsync(url);
                if (stream != null)
                {
                    using var fileStream = new FileStream(path, FileMode.OpenOrCreate);
                    await stream.CopyToAsync(fileStream);
                }
            }
            catch (Exception ex)
            {
                DebugLog("Error in {VkArchiver}:\n" + ex.Message, nameof(VkArchiver));
            }
        }

        static void DebugLog(string format, params object?[] arg)
        {
            Console.WriteLine(format, arg);
        }
    }
}

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

        public VkArchiver(string login, string password, ulong applicationId = 8206863)
        {
            Login = login;
            Password = password;
            ApplicationId = applicationId;
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
                DebugLog("Error in VkArchiver:\n" + ex.Message);
            }
        }

        public async Task ArchivePhotosAsync(From from, string path)
        {
            switch (from)
            {
                case From.All:
                    await ArchiveAllPhotosAsync(path);
                    break;
                case From.Dialogs:
                    await ArchiveDialogsPhotosAsync(path);
                    break;
                case From.Chats:
                    await ArchiveChatsPhotosAsync(path);
                    break;
            }
        }

        public async Task ArchivePhotosAsync(string name, string path)
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
                            await PhotoDownloadAsync(Item.Conversation.Peer.Id, path + @"\" + Item.Conversation.ChatSettings.Title);
                            break;
                        }

                        if (Item.Conversation.Peer.Type.ToString() == "user")
                        {
                            var user = _vkApi.Users.Get(new long[] { Item.Conversation.Peer.Id });
                            var userName = user[0].FirstName != "DELETED" ? $"{user[0].FirstName} {user[0].LastName}" : $"{Item.Conversation.Peer.Id}";

                            if ($"{user[0].FirstName} {user[0].LastName}" == name)
                            {
                                await PhotoDownloadAsync(Item.Conversation.Peer.Id, path + @"\" + userName);
                                break;
                            }
                        }
                    }
                }
                while (count >= 200);
            }
            catch (Exception ex)
            {
                DebugLog("Error in VkArchiver:\n" + ex.Message);
            }
        }

        /// <summary>
        /// Сохраняет все фотографии из всех чатов и диалогов
        /// </summary>
        /// <param name="path">Путь для сохранения фотографий</param>
        private async Task ArchiveAllPhotosAsync(string path)
        {
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
                        if (Item.Conversation.Peer.Type.ToString() == "chat")
                        {
                            await PhotoDownloadAsync(Item.Conversation.Peer.Id, path + @"\" + Item.Conversation.ChatSettings.Title);
                        }

                        if (Item.Conversation.Peer.Type.ToString() == "user")
                        {
                            var user = _vkApi.Users.Get(new long[] { Item.Conversation.Peer.Id });
                            var userName = user[0].FirstName != "DELETED" ? $"{user[0].FirstName} {user[0].LastName}" : $"{Item.Conversation.Peer.Id}";
                            await PhotoDownloadAsync(Item.Conversation.Peer.Id, path + @"\" + userName);
                        }

                    }
                }
                while (count >= 200);
            }
            catch (Exception ex)
            {
                DebugLog("Error in VkArchiver:\n" + ex.Message);
            }
        }

        /// <summary>
        /// Сохраняет все фотографии из всех диалогов
        /// </summary>
        /// <param name="path">Путь для сохранения фотографий</param>
        private async Task ArchiveDialogsPhotosAsync(string path)
        {
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
                        if (Item.Conversation.Peer.Type.ToString() == "user")
                        {
                            var user = _vkApi.Users.Get(new long[] { Item.Conversation.Peer.Id });
                            var userName = user[0].FirstName != "DELETED" ? $"{user[0].FirstName} {user[0].LastName}" : $"{Item.Conversation.Peer.Id}";
                            await PhotoDownloadAsync(Item.Conversation.Peer.Id, path + @"\" + userName);
                        }
                    }
                }
                while (count >= 200);
            }
            catch (Exception ex)
            {
                DebugLog("Error in VkArchiver:\n" + ex.Message);
            }
        }

        /// <summary>
        /// Сохраняет все фотографии из всех чатов
        /// </summary>
        /// <param name="path">Путь для сохранения фотографий</param>
        private async Task ArchiveChatsPhotosAsync(string path)
        {
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
                        if (Item.Conversation.Peer.Type.ToString() == "chat")
                        {
                            await PhotoDownloadAsync(Item.Conversation.Peer.Id, path + @"\" + Item.Conversation.ChatSettings.Title);
                        }
                    }
                }
                while (count >= 200);
            }
            catch (Exception ex)
            {
                DebugLog("Error in VkArchiver:\n" + ex.Message);
            }
        }

        /// <summary>
        /// Скачивает все фотографии из чата/диалога
        /// </summary>
        /// <param name="id">Индификатор чата/диалога</param>
        /// <param name="path">Путь для сохранения фотографий</param>
        private async Task PhotoDownloadAsync(long id, string path)
        {
            try
            {
                string next = string.Empty;
                Directory.CreateDirectory(path);

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
                        var pathFile = path + @"\" + photo.Sizes[^1].Url.Segments[^1];

                        using var stream = await new HttpClient().GetStreamAsync(photo.Sizes[^1].Url);
                        if (stream != null)
                        {
                            using var fileStream = new FileStream(pathFile, FileMode.OpenOrCreate);
                            await stream.CopyToAsync(fileStream);
                        }
                    }
                }
                while (next != null);

                ZipFile.CreateFromDirectory(path, path.Remove(path.Length - 1) + ".zip");
                Directory.Delete(path, true);
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2147024773) 
                    await PhotoDownloadAsync(id, Filter(path));
                else
                    DebugLog("Error in VkArchiver:\n" + ex.Message);
            }
        }

        /// <summary>
        /// Фильтр спецсимволов для создания директории
        /// </summary>
        /// <param name="str">Строка, для фильтрации</param>
        /// <returns>Отфильтрованная строка</returns>
        private static string Filter(string str)
        {
            var charsToRemove = new List<char>() { '/', ':', '*', '?', '"', '<', '>', '|' };
            charsToRemove.ForEach(c => str = str.Replace(c.ToString(), String.Empty));
            str = str.Insert(str.IndexOf('\\'), ":");
            return str;
        }

        static void DebugLog(string format)
        {
            Console.WriteLine(format);
        }
    }
}

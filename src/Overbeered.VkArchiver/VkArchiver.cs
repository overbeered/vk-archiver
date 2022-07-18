using Microsoft.Extensions.DependencyInjection;
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

        public VkArchiver(string login, string password, ulong applicationId)
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

        public async Task<IEnumerable<Peer>> ArchivePhotosAsync(From from)
        {
            return from switch
            {
                From.All => await ArchiveAllPhotosAsync(),
                From.Dialogs => await ArchiveDialogsPhotosAsync(),
                From.Chats => await ArchiveChatsPhotosAsync(),
                _ => new List<Peer>(),
            };
        }

        public async Task<Peer?> ArchivePhotosAsync(string name)
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
                            return new Peer(Item.Conversation.ChatSettings.Title,
                                await PeerPhotoDownloadAsync(Item.Conversation.Peer.Id));
                        }

                        if (Item.Conversation.Peer.Type.ToString() == "user")
                        {
                            var user = _vkApi.Users.Get(new long[] { Item.Conversation.Peer.Id });
                            var userName = user[0].FirstName != "DELETED" ? $"{user[0].FirstName} {user[0].LastName}" : $"{Item.Conversation.Peer.Id}";

                            if ($"{user[0].FirstName} {user[0].LastName}" == name)
                            {
                                return new Peer(userName,
                                    await PeerPhotoDownloadAsync(Item.Conversation.Peer.Id));
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
            return null;
        }

        /// <summary>
        /// Возвращает все фотографии из всех чатов и диалогов
        /// </summary>
        /// <returns>Чаты/диалоги с файлами</returns>
        private async Task<IEnumerable<Peer>> ArchiveAllPhotosAsync()
        {
            ulong? offset = 0;
            long? count;
            var listPeers = new List<Peer>();
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
                            listPeers.Add(new Peer(Item.Conversation.ChatSettings.Title,
                                await PeerPhotoDownloadAsync(Item.Conversation.Peer.Id)));
                        }

                        if (Item.Conversation.Peer.Type.ToString() == "user")
                        {
                            var user = _vkApi.Users.Get(new long[] { Item.Conversation.Peer.Id });
                            var userName = user[0].FirstName != "DELETED" ? $"{user[0].FirstName} {user[0].LastName}" : $"{Item.Conversation.Peer.Id}";
                            listPeers.Add(new Peer(userName,
                                await PeerPhotoDownloadAsync(Item.Conversation.Peer.Id)));
                        }

                    }
                }
                while (count >= 200);
            }
            catch (Exception ex)
            {
                DebugLog("Error in VkArchiver:\n" + ex.Message);
            }

            return listPeers;
        }

        /// <summary>
        /// Возвращает все фотографии из всех диалогов
        /// </summary>
        /// <returns>Диалоги с файлами</returns>
        private async Task<IEnumerable<Peer>> ArchiveDialogsPhotosAsync()
        {
            ulong? offset = 0;
            long? count;
            var listPeers = new List<Peer>();
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
                            listPeers.Add(new Peer(userName,
                                await PeerPhotoDownloadAsync(Item.Conversation.Peer.Id)));
                        }
                    }
                }
                while (count >= 200);
            }
            catch (Exception ex)
            {
                DebugLog("Error in VkArchiver:\n" + ex.Message);
            }

            return listPeers;
        }

        /// <summary>
        /// Возвращает все фотографии из всех чатов
        /// </summary>
        /// <returns>Чаты с файлами</returns>
        private async Task<IEnumerable<Peer>> ArchiveChatsPhotosAsync()
        {
            ulong? offset = 0;
            long? count;
            var listPeers = new List<Peer>();
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
                            listPeers.Add(new Peer(Item.Conversation.ChatSettings.Title,
                                await PeerPhotoDownloadAsync(Item.Conversation.Peer.Id)));
                        }
                    }
                }
                while (count >= 200);
            }
            catch (Exception ex)
            {
                DebugLog("Error in VkArchiver:\n" + ex.Message);
            }

            return listPeers;
        }

        /// <summary>
        /// Скачивает все фотографии из чата/диалога
        /// </summary>
        /// <param name="id">Индификатор чата/диалога</param>
        /// <returns>Файлы загрузки</returns>
        private async Task<IEnumerable<FileDownload>> PeerPhotoDownloadAsync(long id)
        {
            string next = string.Empty;
            var listFileDownload = new List<FileDownload>();

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
                        listFileDownload.Add(new FileDownload(photo.Sizes[^1].Url.Segments[^1],
                                await new HttpClient().GetStreamAsync(photo.Sizes[^1].Url)));
                    }
                }
                while (next != null);
            }
            catch (Exception ex)
            {
                DebugLog("Error in VkArchiver:\n" + ex.Message);
            }

            return listFileDownload;
        }

        static void DebugLog(string format, params object?[] arg)
        {
            Console.WriteLine(format, arg);
        }
    }
}

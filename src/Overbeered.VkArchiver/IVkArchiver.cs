using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Overbeered.VkArchiver
{
    /// <summary>
    /// Архиватор
    /// </summary>
    public interface IVkArchiver
    {
        /// <summary>
        /// Aрхивирование всех фото из всех чатов/диалогов в зависимости от флага
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        Task ArchivePhotosAsync(From from);

        /// <summary>
        /// Aрхивирование фото из чата/диалога по его названию
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task ArchivePhotosAsync(string name);
    }
}

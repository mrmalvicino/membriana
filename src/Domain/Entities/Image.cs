using Contracts.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    /// <summary>
    /// Representa una imagen.
    /// </summary>
    public class Image : IIdentifiable
    {
        #region Id
        public int Id { get; set; }
        #endregion

        #region Url
        [Required]
        public string Url { get; set; } = null!;
        #endregion
    }
}

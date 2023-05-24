using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WebApplication1.Models
{
    public class Secret
    {
        public long ID { get; set; }

        /// <summary>
        /// Unique hash to identify the secrets
        /// </summary>
        [NotNull]
        public string? hash { get; set; }

        /// <summary>
        /// The secret itself
        /// </summary>
        /// 
        [DisallowNull]
        public string? secretText { get; set; }

        /// <summary>
        /// The date and time of the creation
        /// </summary>
        public string? createdAt { get; set; }

        /// <summary>
        /// The secret cannot be reached after this time
        /// </summary>
        public int expiresAt { get; set; }

        /// <summary>
        /// How many times the secret can be viewed
        /// </summary>
        public int remainingViews { get; set; } 


        
    }
}

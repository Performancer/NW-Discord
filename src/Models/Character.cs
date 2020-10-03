using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NW.Models
{
    public enum AccessRole
    {
        Player, Counsellor, GameMaster
    }

    public class Character
    {
        Character()
        {
            Id = Guid.NewGuid();
        }

        [StringLength(20, MinimumLength = 1, ErrorMessage = "Account needs to be between 1-20 letters")]
        [JsonIgnore] // not a secret per se, but still the security is appreciated
        public string AccountName { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Name has to be between 1-255 letters")]
        public string Name { get; set; }

        [EnumDataType(typeof(AccessRole))]
        public AccessRole Role { get; set; }

        public Vector3 Location { get; set; }

        public int Score { get; set; }

        public Guid Id { get; set; }
    }
}
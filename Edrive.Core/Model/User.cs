using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Edrive.Core.Model
{
    // Always extend this class using Partials, do not add extra members to this file!

    [DataContract]
    [DisplayColumn("Username")]
    public partial class User : PersistentEntity
    {
        [DataMember]
        public virtual Guid UserId { get; set; }

        [Required]
        [DataMember]
        public virtual String Username { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [DataMember]
        public virtual String Email { get; set; }

        [Required, DataType(DataType.Password)]
        [DataMember]
        public virtual String Password { get; set; }
    
        [DataMember]
        public virtual String FirstName { get; set; }

        [DataMember]
        public virtual String LastName { get; set; }

        [DataType(DataType.MultilineText)]
        public virtual String Comment { get; set; }
        
        [DataMember]
        public virtual Boolean IsApproved { get; set; }

        [DataMember]
        public virtual int PasswordFailuresSinceLastSuccess { get; set; }

        [DataMember]
        public virtual DateTime? LastPasswordFailureDate { get; set; }

        [DataMember]
        public virtual DateTime? LastActivityDate { get; set; }

        [DataMember]
        public virtual DateTime? LastLockoutDate { get; set; }

        [DataMember]
        public virtual DateTime? LastLoginDate { get; set; }

        [DataMember]
        public virtual String ConfirmationToken { get; set; }

        [DataMember]
        public virtual DateTime? CreateDate { get; set; }

        [DataMember]
        public virtual Boolean IsLockedOut { get; set; }

        [DataMember]
        public virtual DateTime? LastPasswordChangedDate { get; set; }

        [DataMember]
        public virtual String PasswordVerificationToken { get; set; }

        [DataMember]
        public virtual DateTime? PasswordVerificationTokenExpirationDate { get; set; }

        [DataMember]
        public virtual ICollection<Role> Roles { get; set; }
    }
}
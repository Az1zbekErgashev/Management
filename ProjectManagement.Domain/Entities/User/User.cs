﻿using ProjectManagement.Domain.Commons;
using ProjectManagement.Domain.Enum;

namespace ProjectManagement.Domain.Entities.User;
public class User : Auditable
{
    public required string Email { get; set; }
    public required string Name { get; set; }
    public required string Password { get; set; }
    public required string Surname { get; set; }
    public required string PhoneNumber { get; set; }
    public Role IndividualRole { get; set; }
    public virtual Attachment.Attachment? Image { get; set; }
    public virtual ICollection<Logs.Logs> Logs { get; set; }
    public int? ImageId { get; set; }
    public Country.Country? Country { get; set; }
    public int? CountryId { get; set; }
    public DateTime? DateOfBirth { get; set; }
}

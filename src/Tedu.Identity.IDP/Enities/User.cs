﻿using Microsoft.AspNetCore.Identity;

namespace Tedu.Identity.IDP.Enities;

public class User : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
}

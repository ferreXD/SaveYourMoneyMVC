using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;

namespace SaveYourMoneyMVC.Entities;

// Add profile data for application users by adding properties to the User class
public class User : IdentityUser
{
    public string Language { get; set; }
    public virtual List<Gasto> Gastos { get; set; }
}


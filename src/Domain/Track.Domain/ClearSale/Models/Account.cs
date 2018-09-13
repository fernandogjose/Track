using System;
using System.Collections.Generic;

namespace Track.Domain.ClearSale.Models {
    public class Account {
        public int Id { get; set; }
        public String Date { get; set; }
        public string SessionId { get; set; }
        public string Name { get; set; }
        public string PersonDocument { get; set; }
        public String BirthDate { get; set; }
        public string Gender { get; set; }
        public string CompanyName { get; set; }
        public string CompanyDocument { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string OptinEmail { get; set; }
        public string OptinMobile { get; set; }
        public List<Address> Addresses { get; set; }
        public List<Phones> Phones { get; set; }
    }
}
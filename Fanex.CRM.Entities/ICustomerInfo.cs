namespace Fanex.CRM.Entities
{
    interface ICustomerInfo
    {
         int Id { get; set; }

         string Username { get; set; }

         string Title { get; set; }

         string FirstName { get; set; }

         string LastName { get; set; }

         string Phone { get; set; }

         string MobilePhone { get; set; }

         string Fax { get; set; }

         string Email { get; set; }

         Setting Country { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telefonkonyv
{
    class Person
    {
		private int id;
		private string firstName;
		private string lastName;
		private string contact;
		private string email;
		private string address;
		public int Id
		{
			get => id;
			set => id = value;
		}
		public string FirstName
		{
			get => firstName;
			set => firstName = value;
		}
		public string LastName
		{
			get => lastName;
			set => lastName = value;
		}
			public string Contact
		{
			get => contact;
			set => contact = value;
		}
		public string Email
		{
			get => email;
			set => email = value;
		}
		public string Address
		{
			get => address;
			set => address = value;
		}

	}
}

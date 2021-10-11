using AuthenticationService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseServices.DTO
{
    public class MedicDTO
    {
        private string _firstName;
        private string _lastName;
        private int _age;
        private int _semester;
        private string _rotation;
        private AuthDTO authentication;

        public MedicDTO()
        {

        }
         
        public string FirstName { get => _firstName; set => _firstName = value; }
        public string LastName { get => _lastName; set => _lastName = value; }
        public int Age { get => _age; set => _age = value; }
        public int Semester { get => _semester; set => _semester = value; }
        public string Rotation { get => _rotation; set => _rotation = value; }
        public AuthDTO Authentication { get => authentication; set => authentication = value; }
    }
}

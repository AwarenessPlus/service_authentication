using System;

namespace AuthenticationService.DTO
{
    public class MedicSignUpDTO
    {
        private string _firstName;
        private string _lastName;
        private DateTime _birthDate;
        private int _semester;
        private string _rotation;
        private AuthDTO authentication;

        public MedicSignUpDTO()
        {

        }

        public MedicSignUpDTO(string firstName, string lastName, DateTime birthDate, int semester, string rotation, AuthDTO authentication)
        {
            _firstName = firstName;
            _lastName = lastName;
            _birthDate = birthDate;
            _semester = semester;
            _rotation = rotation;
            this.authentication = authentication;
        }

        public string FirstName { get => _firstName; set => _firstName = value; }
        public string LastName { get => _lastName; set => _lastName = value; }
        public int Semester { get => _semester; set => _semester = value; }
        public string Rotation { get => _rotation; set => _rotation = value; }
        public AuthDTO Authentication { get => authentication; set => authentication = value; }
        public DateTime BirthDate { get => _birthDate; set => _birthDate = value; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace library
{
    public class Member
    {
        private const int MaxBorrowLimit = 5;
        private string Name;
        private List<int> borrowedBookIDs;

        public Member(string name)
        {
            this.Name = name;
            borrowedBookIDs = new List<int>();
        }

        public string getName() { return this.Name; }
        public void BorrowBook(int bookID)
        {
            if (borrowedBookIDs.Count >= MaxBorrowLimit)
            {
                throw new InvalidOperationException("Member has reached the borrowing limit.");
            }

            borrowedBookIDs.Add(bookID);
        }

        public void ReturnBook(int bookID)
        {
            if (!borrowedBookIDs.Contains(bookID))
            {
                throw new InvalidOperationException("Member has not borrowed the book.");
            }

            borrowedBookIDs.Remove(bookID);
        }

        public IReadOnlyList<int> GetBorrowedBookIDs()
        {
            return borrowedBookIDs.AsReadOnly();
        }


    }
}

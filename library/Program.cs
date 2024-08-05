using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace library
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string filePath = "input.txt";  // Specify the path to your input.
            Library library = Library.Instance;

            using (var sr = new StreamReader(filePath))
            {
                while (sr.Peek() >= 0)  // Read until there are no more characters to be read
                {
                    string line = sr.ReadLine();
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        string command = line.Substring(0, 1);    // Get the first character of the line as the command.
                        string rest = line.Substring(1);          // Remaining part of the line.

                        switch (command)
                        {
                            case "A":
                                // Add a book. The book data is on the same line, separated by commas.
                                try
                                {
                                    library.AddBook(Book.FromTxt(rest));
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Error: {ex.Message}");
                                }
                                break;

                            case "M":
                                // Add a Member. The member's name is on the next line.
                                string memberName = sr.ReadLine();
                                library.AddMember(new Member(memberName));
                                break;

                            case "B":
                                // Borrow a Book. The member's name is on the next line, followed by the book ID and the number of days to borrow for.
                                string borrowerName = sr.ReadLine();
                                int bookId = int.Parse(sr.ReadLine());
                                int daysToBorrow = int.Parse(sr.ReadLine());
                                Member memberToBorrow = library.FindMemberByName(borrowerName);
                                try
                                {
                                    library.BorrowBook(memberToBorrow, bookId, DateTime.Now.AddDays(daysToBorrow));
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Error: {ex.Message}");
                                }
                                break;

                            case "R":
                                // Return a Book. The member's name is on the next line, followed by the book ID.
                                string returnerName = sr.ReadLine();
                                int returnBookId = int.Parse(sr.ReadLine());
                                Member memberToReturn = library.FindMemberByName(returnerName);
                                try
                                {
                                    library.ReturnBook(memberToReturn, returnBookId);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Error: {ex.Message}");
                                }
                                break;

                            case "C":
                                // Calculate a Member's late fee. The member's name is on the next line.
                                string memberNameLateFee = sr.ReadLine();
                                Member memberForLateFee = library.FindMemberByName(memberNameLateFee);
                                decimal lateFee = library.CalculateLateFee(memberForLateFee);
                                Console.WriteLine($"The late fee for {memberNameLateFee} is {lateFee}.");
                                break;

                            default:
                                throw new Exception($"Unrecognized command: {command}");
                        }
                    }
                }
            }
        }
    }
}
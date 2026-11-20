namespace Models;

public record Student(int id, String FirstName, String MiddleName, String LastName)
{
    public override string ToString() => $"{LastName.Trim()}, {FirstName.Trim()} {MiddleName.Trim()[0]}";
}

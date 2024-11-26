[Flags]
public enum Permission
{
  None = 0,
  ReadOwnCharacters = 1,
  WriteOwnCharacters = 2,
  ReadAllCharacters = 4,
  WriteAllCharacters = 8,
  ManageUsers = 16,
  AdministerSystem = 32,
  WriteSkills = 1 << 6,          // 64
  WriteStatModifiers = 1 << 7    // 128

}

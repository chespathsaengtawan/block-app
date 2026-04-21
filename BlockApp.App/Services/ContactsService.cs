using Microsoft.Maui.ApplicationModel.Communication;

namespace BlockApp.App.Services;

public class ContactsService
{
    public async Task<List<ContactDto>> GetAllContactsAsync()
    {
        try
        {
            var status = await Permissions.RequestAsync<Permissions.ContactsRead>();
            if (status != PermissionStatus.Granted)
                return [];

            var contacts = await Microsoft.Maui.ApplicationModel.Communication.Contacts.Default.GetAllAsync();
            return contacts
                .Select(c => new ContactDto
                {
                    DisplayName = c.DisplayName ?? string.Empty,
                    PhoneNumbers = c.Phones
                        .Select(p => p.PhoneNumber ?? string.Empty)
                        .Where(n => !string.IsNullOrWhiteSpace(n))
                        .ToList()
                })
                .Where(c => c.PhoneNumbers.Count > 0)
                .OrderBy(c => c.DisplayName)
                .ToList();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[ContactsService] Error: {ex.Message}");
            return [];
        }
    }
}

public class ContactDto
{
    public string DisplayName { get; set; } = string.Empty;
    public List<string> PhoneNumbers { get; set; } = [];
}

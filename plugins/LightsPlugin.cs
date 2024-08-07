// This is the plug in. It uses an array of 20 lights that may be controlled
// each of  the kernel functions have been added as more use cases are tried in the agent

using System.ComponentModel;
using Microsoft.SemanticKernel;

public class LightsPlugin
{
    LightModel[] _lights = new LightModel[20];

    public LightsPlugin()
    {
        for (int i = 0; i < 20; i++)
        {
            _lights[i] = new LightModel
            {
                ID = i,
                Name = $"Light {i}",
                IsOn = false,
                Brightness = 0
            };
        }
    }

    [KernelFunction("get_light_name")]
    [Description("gets a light name by its ID.")]
    public async Task<string> GetLightName(
        Kernel kernel,
        int id
    )
    {
        // Add logic to send an email using the recipientEmails, subject, and body
        // For now, we'll just print out a success message to the console
        var name = _lights[id].Name;
        Console.WriteLine(name);
        return name;
    }

    [KernelFunction("get_light_brightness")]
    [Description("gets a light brightness by its ID.")]
    public async Task<int> GetLightBrightness(
        Kernel kernel,
        int id
    )
    {
        // Add logic to send an email using the recipientEmails, subject, and body
        // For now, we'll just print out a success message to the console
        var brightness = _lights[id].Brightness;
        Console.WriteLine(brightness);
        return brightness;
    }

    [KernelFunction("set_light_brightness")]
    [Description("sets a light brightness by its ID.")]
    public async Task SetLightBrightness(
        Kernel kernel,
        int id,
        int brightness
    )
    {
        // Add logic to send an email using the recipientEmails, subject, and body
        // For now, we'll just print out a success message to the console
        _lights[id].Brightness = brightness;
        Console.WriteLine("Brightness set!");
    }

    [KernelFunction("turn_on_light")]
    [Description("turns on a light by its ID.")]
    public async Task TurnOnLight(
        Kernel kernel,
        int id
    )
    {
        // Add logic to send an email using the recipientEmails, subject, and body
        // For now, we'll just print out a success message to the console
        _lights[id].IsOn = true;
        Console.WriteLine("Light turned on!");
    }

    [KernelFunction("rename_light")]
    [Description("renames a light by its ID.")]
    public async Task RenameLight(
        Kernel kernel,
        int id,
        string name
    )
    {
        // Add logic to send an email using the recipientEmails, subject, and body
        // For now, we'll just print out a success message to the console
        _lights[id].Name = name;
        Console.WriteLine("Light renamed!");
    }

    [KernelFunction("find_light_by_name")]
    [Description("finds a light by its name.")]
    public async Task<int> FindLightByName(
        Kernel kernel,
        string name
    )
    {
        // Add logic to send an email using the recipientEmails, subject, and body
        // For now, we'll just print out a success message to the console
        for (int i = 0; i < 20; i++)
        {
            if (_lights[i].Name == name)
            {
                Console.WriteLine(i);
                return i;
            }
        }
        Console.WriteLine(-1);
        return -1;
    }

    [KernelFunction("list_ids_of_lights")]
    [Description("lists the IDs of all lights.")]
    public async Task<List<int>> ListIDsOfLights(
        Kernel kernel
    )
    {
        // Add logic to send an email using the recipientEmails, subject, and body
        // For now, we'll just print out a success message to the console
        List<int> ids = new List<int>();
        for (int i = 0; i < 20; i++)
        {
            ids.Add(i);
        }
        Console.WriteLine(ids);
        return ids;
    }

    [KernelFunction("get_the_light_onoff_status")]
    [Description("gets the on/off status of a light by its ID.")]
    public async Task<bool> GetTheLightOnOffStatus(
        Kernel kernel,
        int id
    )
    {
        // Add logic to send an email using the recipientEmails, subject, and body
        // For now, we'll just print out a success message to the console
        var isOn = _lights[id].IsOn;
        Console.WriteLine(isOn);
        return isOn;
    }
}

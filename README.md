# A Semantic kernel planner demonstration

This is an implementation of the sample as descibed in the [learn module](https://learn.microsoft.com/en-us/semantic-kernel/concepts/planning?pivots=programming-language-csharp) on semantic kernel planners. The learn page describes the use case of a virtual set of lamps that may be controlled. The learn module did not provide an implementation of the plugin for this, so this is what this repo seeks to address.

## What's in it for me?
This repo provides an easy demonstration of the power of planning in Semantic kernel. 

Plugins as a means of calling out to external services have been around for a while and these are based on [OpenAI function calling](https://platform.openai.com/docs/guides/function-calling). Plugins really  powerful, but often there are series of steps are needed to accomplish a real task. This can be [messy](https://learn.microsoft.com/en-us/azure/ai-services/openai/how-to/function-calling) without a planner.

It is hoped that this repo allows you to see how automatic functions and plugins can be coordinated in a planner - with very little code. The coordination or orchestration is done by the LLM, potentially calling into the plugin's *Kernel Functions* repeatedly to accomplish the user's goal.

Give it a try!

## Semantic Kernel Planners
In previous iterations of semantic kernel, plans were their own distinct concept. With the advent of automatic function calling, now semantic kernel accomplishes planning using automatic function calling.

The planning loop is described in the first link above, but is really quite simple:

````
string? userInput;
do {
    // Collect user input
    Console.Write("User > ");
    userInput = Console.ReadLine();

    // Add user input
    history.AddUserMessage(userInput);

    // 3. Get the response from the AI with automatic function calling
    var result = await chatCompletionService.GetChatMessageContentAsync(
        history,
        executionSettings: openAIPromptExecutionSettings,
        kernel: kernel);

    // Print the results
    Console.WriteLine("Assistant > " + result);

    // Add the message from the agent to the chat history
    history.AddMessage(result.Role, result.Content ?? string.Empty);
} while (userInput is not null);
````

The chat "history" is an important part of this as this provides a continuing context for the LLM to iterate over the user ask and to potentially call plugins a number of times in order to reach the goal the user asks for. For this to hang together, automatic function calling had to previously be enabled.

````
// 2. Enable automatic function calling
OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new() 
{
    ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
};
````

## The Lights Plugin
Another part of the learn documentation has an implementation of as lights pluging [here](https://learn.microsoft.com/en-us/semantic-kernel/concepts/plugins/adding-native-plugins?pivots=programming-language-csharp) but I built my own.

This is where all of the functions get called and is a simple model of a light:
````
public class LightModel
{
    public int ID { get; set; }
    public string Name { get; set; }
    public bool IsOn { get; set; }
    public int Brightness { get; set; }
}
````
and a series of methods - each of which is a *KernelFunction* that act upon an array of these light objects.

This is a very simplistic implementation, but has surprising power when used with the semantic kernel planner.

### The kernel functions exposed
I originally started with a minimal set of kernel functions, but expanded these once trying some of the different use cases

1. get_light_name
2. get_light_brightness
3. set_light_brightness
4. turn_on_light
5. rename_light
6. find_light_by_name
7. list_ids_of_lights
8. get_the_light_onoff_status

## Sample conversation
Below is a sample conversation. Note how it keeps track of what was previously asked and in some cases is making multiple kernel function calls to achieve the user goal

````
User > what is the birhtness of light ID 1
0
Assistant > The brightness of the light with ID 1 is 0.
User > can you set that to 50
Brightness set!
Assistant > The brightness of the light with ID 1 has been set to 50.
User > can you rename this light "Johns office"
Light renamed!
Assistant > The light with ID 1 has been successfully renamed to "Johns office".
User > can you list the names of the first 5 lights
System.Collections.Generic.List`1[System.Int32]
Light 0
Johns office
Light 2
Light 3
Light 4
Assistant > The names of the first five lights are:

1. Light 0
2. Johns office
3. Light 2
4. Light 3
5. Light 4
User > and their brightness? 
0
50
0
0
0
Assistant > The brightness levels of the first five lights are:

1. Light 0: 0
2. Johns office: 50
3. Light 2: 0
4. Light 3: 0
5. Light 4: 0
User > can you toggle all of the lights
False
False
False
False
False
False
False
False
False
False
False
False
False
False
False
False
False
False
False
False
Light turned on!
Light turned on!
Light turned on!
Light turned on!
Light turned on!
Light turned on!
Light turned on!
Light turned on!
Light turned on!
Light turned on!
Light turned on!
Light turned on!
Light turned on!
Light turned on!
Light turned on!
Light turned on!
Light turned on!
Light turned on!
Light turned on!
Light turned on!
Assistant > All the lights have been successfully toggled on.
User > what is the status of all of the lights now
True
True
True
True
True
True
True
True
True
True
True
True
True
True
True
True
True
True
True
True
Assistant > The status of all the lights is now "on."
User > can you set Johns office to off
True
Light turned on!
Assistant > It appears there isn't a direct "turn off" function available to me either. I attempted to toggle the light, but it seems the necessary function wasn't found.

Would you like me to take any further steps? Please let me know how you’d like to proceed.
User >
````

This final step in the conversation is really interesting. The LLM does not know how to proceed and it is giving really good hints how the set of kernel functions can be extended to help with this use case.

````
User > set light ID 2 to 100
Brightness set!
Assistant > The brightness of the light with ID 2 has been set to 100.
User > can you add up the birghtness levels of all of the lights
0
50
100
0
0
0
0
0
0
0
0
0
0
0
0
0
0
0
0
0
Assistant > The total brightness level of all the lights combined is now 150.
````
Another illustration of the LLM making multiple calls to get to the answer.


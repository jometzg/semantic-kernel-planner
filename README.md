# A Semantic kernel planner demonstration

This is an implementation of the sample as descibed in the [learn module](https://learn.microsoft.com/en-us/semantic-kernel/concepts/planning?pivots=programming-language-csharp) on semantic kernel planners. The learn page describes the use case of a virtual set of lamps that may be controlled. The learn module did not provide an implementation of the plugin for this, so this is what this repo seeks to address.

## What's in it for me?
This repo provides an easy demonstration of the power of planning in Semantic kernel. 

Plugins as a means of calling out to external services have been around for a while and these are based on [OpenAI function calling](https://platform.openai.com/docs/guides/function-calling). Plugins really really powerful, but often a series of steps may be needed to accomplish a real task and this is really messy without a planner.

It is hoped that this repo allows you to see how automatic functions and plugins can be coordinated in a planner - with very little code. The coordination or orchestration is done by the LLM, potentially calling into the plugin's *Kernel Functions* repeatidly to accomplish a goal.

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

## Some sample conversations

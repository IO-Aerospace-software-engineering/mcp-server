using Markdig;
using Microsoft.Extensions.AI;
using ChatMessage = Microsoft.Extensions.AI.ChatMessage;

namespace IO.MCP.AI;

public class AIServices
{
    public const char LOADER = '\u23fa';
    private readonly IChatClient _chatClient;

    private readonly List<ChatMessage> _messages = new();
    public IReadOnlyCollection<ChatMessage> Messages => _messages;
    private readonly ChatOptions _chatOptions;

    public event Action<string> MessageReceived;
    public event Action MessageCompleted;

    const string SYSTEM_DEFINITION =
        "Your name is IO Assistant, you are an expert in astronomy, astrodynamics, matematics and physics, you've been created by Sylvain Guillet." +
        "You must always precise units of measure used if needed." +
        "You must always represent a collection of data as table, each row doesn't contain new line." +
        "You provide concise and accurate answers to the user's questions. If you don't know the answer, you must say so. But you can ask to the user if he wants more explanation or demonstration";

    public AIServices(IChatClient chatClient)
    {
        _chatClient = chatClient ?? throw new ArgumentNullException(nameof(chatClient));
        _messages.Add(new ChatMessage(ChatRole.System, SYSTEM_DEFINITION));
        var aiFunctionFactoryCreateOptions = new AIFunctionFactoryOptions
            { AdditionalProperties = new Dictionary<string, object>() { { "Strict", false } } };
        _chatOptions = new ChatOptions
        {
            Temperature = 0.1f,
            TopK = 5,
            TopP = 0.95f,
            AdditionalProperties = new() { ["seed"] = 0 },
            Tools = new List<AITool>
            {
                AIFunctionFactory.Create(Tools.CelestialBodyTools.GetEphemerisAsStateVectors, aiFunctionFactoryCreateOptions),
                AIFunctionFactory.Create(Tools.CelestialBodyTools.GetCelestialBodyProperties, aiFunctionFactoryCreateOptions),
                AIFunctionFactory.Create(Tools.TimeTools.ConvertDateTime, aiFunctionFactoryCreateOptions),
                AIFunctionFactory.Create(Tools.TimeTools.CurrentDateTime, aiFunctionFactoryCreateOptions),
                AIFunctionFactory.Create(Tools.OrbitalParametersTools.ConvertStateVectorToEquinoctialElements, aiFunctionFactoryCreateOptions),
                AIFunctionFactory.Create(Tools.OrbitalParametersTools.ConvertStateVectorToKeplerianElements, aiFunctionFactoryCreateOptions),
                AIFunctionFactory.Create(Tools.OrbitalParametersTools.ConvertKeplerianElementsToStateVector, aiFunctionFactoryCreateOptions),
                AIFunctionFactory.Create(Tools.OrbitalParametersTools.ConvertEquinoctialElementsToStateVector, aiFunctionFactoryCreateOptions),
                AIFunctionFactory.Create(Tools.OrbitalParametersTools.ConvertStateVectorToTheGivenFrame, aiFunctionFactoryCreateOptions),
                AIFunctionFactory.Create(Tools.OrbitalParametersTools.ConvertStateVectorToEquatorialCoordinates, aiFunctionFactoryCreateOptions),
                AIFunctionFactory.Create(Tools.GeometryFinderTools.FindCoordinateConstraint, aiFunctionFactoryCreateOptions),
                AIFunctionFactory.Create(Tools.GeometryFinderTools.FindDistanceConstraint, aiFunctionFactoryCreateOptions),
                AIFunctionFactory.Create(Tools.GeometryFinderTools.FindOccultingConstraint, aiFunctionFactoryCreateOptions),
                AIFunctionFactory.Create(Tools.SiteTools.GetDeepSpaceStationPlanetodeticCoordinates, aiFunctionFactoryCreateOptions),
                AIFunctionFactory.Create(Tools.SiteTools.GetDeepSpaceStationStateVector, aiFunctionFactoryCreateOptions),
                AIFunctionFactory.Create(Tools.SiteTools.GetHorizontalCoordinates, aiFunctionFactoryCreateOptions),
                AIFunctionFactory.Create(Tools.SiteTools.GetDSSFrame, aiFunctionFactoryCreateOptions),
                AIFunctionFactory.Create(Tools.MathTools.DegreesToRadians, aiFunctionFactoryCreateOptions),
                AIFunctionFactory.Create(Tools.MathTools.RadiansToDegrees, aiFunctionFactoryCreateOptions),
                AIFunctionFactory.Create(Tools.MathTools.ConvertDegreesToHours, aiFunctionFactoryCreateOptions),
                AIFunctionFactory.Create(Tools.MathTools.ConvertHoursToDegrees, aiFunctionFactoryCreateOptions),
                AIFunctionFactory.Create(Tools.MathTools.DegreesToArcseconds, aiFunctionFactoryCreateOptions),
                AIFunctionFactory.Create(Tools.MathTools.ArcsecondsToDegrees, aiFunctionFactoryCreateOptions),
                AIFunctionFactory.Create(Tools.MathTools.RadiansToArcseconds, aiFunctionFactoryCreateOptions),
                AIFunctionFactory.Create(Tools.MathTools.ArcsecondsToRadians, aiFunctionFactoryCreateOptions),
                AIFunctionFactory.Create(Tools.MathTools.MetersToMiles, aiFunctionFactoryCreateOptions),
                AIFunctionFactory.Create(Tools.MathTools.MilesToMeters, aiFunctionFactoryCreateOptions),
                AIFunctionFactory.Create(Tools.MathTools.MetersToFeet, aiFunctionFactoryCreateOptions),
                AIFunctionFactory.Create(Tools.MathTools.FeetToMeters, aiFunctionFactoryCreateOptions),
                AIFunctionFactory.Create(Tools.MathTools.MetersToKilometers, aiFunctionFactoryCreateOptions),
                AIFunctionFactory.Create(Tools.MathTools.KilometersToMeters, aiFunctionFactoryCreateOptions),
                AIFunctionFactory.Create(Tools.MathTools.MetersToAstronomicalUnits, aiFunctionFactoryCreateOptions),
                AIFunctionFactory.Create(Tools.MathTools.AstronomicalUnitsToMeters, aiFunctionFactoryCreateOptions),
                AIFunctionFactory.Create(Tools.MathTools.MetersToLightYears, aiFunctionFactoryCreateOptions),
                AIFunctionFactory.Create(Tools.MathTools.LightYearsToMeters, aiFunctionFactoryCreateOptions),
                AIFunctionFactory.Create(Tools.MathTools.MetersToParsec, aiFunctionFactoryCreateOptions),
                AIFunctionFactory.Create(Tools.MathTools.ParsecToMeters, aiFunctionFactoryCreateOptions),
                
            }
        };
    }

    public async Task SendAsync(string message)
    {
        _messages.Add(new ChatMessage(ChatRole.User, message));
        MessageReceived?.Invoke("");

        // Add an empty placeholder for the assistant's reply
        var assistantMessage = new ChatMessage(ChatRole.Assistant, string.Empty);
        _messages.Add(assistantMessage);
        MessageReceived?.Invoke(""); // Update UI with initial empty assistant message

        // Accumulate content from streaming updates
        await foreach (var msg in _chatClient.GetStreamingResponseAsync(_messages, _chatOptions))
        {
            assistantMessage = new ChatMessage(ChatRole.Assistant, assistantMessage.Text + msg.Text);
            _messages[_messages.Count - 1] = assistantMessage;
            MessageReceived?.Invoke(msg.Text); // Trigger UI update with the progressively updated message
        }

        assistantMessage = new ChatMessage(ChatRole.Assistant, assistantMessage.Text?.Replace(LOADER, ' '));
        _messages[_messages.Count - 1] = assistantMessage;
        MessageCompleted?.Invoke();
    }
    
    public async IAsyncEnumerable<string> Send(IEnumerable<Models.ChatMessage> messages)
    {
        var aiMessages = messages.Select(m => new ChatMessage(new ChatRole(m.Role), m.Message)).ToList();
        await foreach(var msg in _chatClient.GetStreamingResponseAsync(aiMessages, _chatOptions))
        {
            yield return msg.Text;
        }
    }

    static string PrepareMathForMarkdig(string input)
    {
        return input.Replace(@"\", @"\\");
    }

    public static string ConvertMarkdownToHtml(string markdown)
    {
        // Use Markdig pipeline with table support
        var pipeline = new Markdig.MarkdownPipelineBuilder()
            .UsePipeTables()
            .Build();

        return Markdig.Markdown.ToHtml(PrepareMathForMarkdig(markdown), pipeline);
    }

    public void Clear()
    {
        _messages.Clear();
        _messages.Add(new ChatMessage(ChatRole.System, SYSTEM_DEFINITION));
    }
}
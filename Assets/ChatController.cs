using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using OpenAI;
using System;
using OpenAI.Assistants;
using OpenAI.Threads;
using Utilities.WebRequestRest.Interfaces;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine.Events;
using OpenAI.Models;

public class ChatController : MonoBehaviour
{
    private UIDocument uIDocument;
    private ScrollView scrollView;
    private TextField input;
    private Button sendButton;

    private List<Message> messages = new List<Message>();
    private OpenAIClient api;
    private AssistantResponse assistant;
    private AssistantResponse interpreter;
    public UnityAction<List<Message>> OnResponse;
    public UnityAction OnContinueDate;
    public String language = "English";
    public ChatTopicsScriptableObject topicsSO;
    public int keepDatingCount;
    
    private string topics;
    private bool keepDatingWasCalled;

    // Start is called before the first frame update
    async void Start()
    {
        uIDocument = GetComponent<UIDocument>();
        scrollView = uIDocument.rootVisualElement.Q<ScrollView>("ScrollView");
        input = uIDocument.rootVisualElement.Q<TextField>("input");
        sendButton = uIDocument.rootVisualElement.Q<Button>("SendButton");
        api = new OpenAIClient(new OpenAIAuthentication().LoadFromPath("./openai.json"));

        sendButton.clicked += SendButton_clicked;

       // await api.AssistantsEndpoint.CreateAssistantAsync(new CreateAssistantRequest(model: Model.GPT4o, name: "Interpreter", instructions: "Does the user intent to proceed the tour/date?\r\nAnswer only with yes or no "));
        interpreter = await api.AssistantsEndpoint.RetrieveAssistantAsync("asst_kBbsUX3sNnIE1lrRjFCrfDzm");
        assistant = await api.AssistantsEndpoint.RetrieveAssistantAsync("asst_lV1unkHA5iPZDFsANkY7APJD");

        topicsSO = ScriptableObject.Instantiate(topicsSO);

        OnContinueDate += Test;

        OnResponse += SetMessages;
        OnResponse += TestHandleResponse;

        StartBreak(new() { { "Entrance", 0 }, { "Ceiling Murals", 0 } });
    }

    private void Test()
    {
        Debug.Log("Continue Date");
    }

    private void SetMessages(List<Message> messages)
    {
        this.messages = messages;
    }

    public async void StartBreak(Dictionary<String, int> topicPriorities)
    {
        var conversationEndNote = "";

        topics = PrepareTopics(topicPriorities);
        string instructions = PrepareInstructions(conversationEndNote, topics);

        await assistant.ModifyAsync(new CreateAssistantRequest(instructions: instructions));

        if (messages.Count == 0)
        {
            messages.Add(new Message("Start of the break"));
            ThreadResponse thread = null;

            async Task StreamEventHandler(IServerSentEvent streamEvent)
            {
                switch (streamEvent)
                {
                    case ThreadResponse threadResponse:
                        thread = threadResponse;
                        break;
                    case RunResponse runResponse:
                        if (runResponse.Status == RunStatus.RequiresAction)
                        {
                            var toolOutputs = await assistant.GetToolOutputsAsync(runResponse);

                            foreach (var toolOutput in toolOutputs)
                            {
                                Debug.Log($"Tool Output: {toolOutput}");
                            }

                            await runResponse.SubmitToolOutputsAsync(toolOutputs, StreamEventHandler);
                        }
                        break;
                    default:
                        Debug.Log("Unhandled:\n" + streamEvent.ToJsonString());
                        break;
                }

                var responseMessages = await thread.ListMessagesAsync();
                List<Message> messages = new List<Message>();

                foreach (var response in responseMessages.Items.Reverse())
                {
                    messages.Add(new Message(response.PrintContent(), response.Role));
                }
               if (messages.Count >= 1 && messages[0].Role == Role.User) messages.RemoveAt(0);

                OnResponse(messages);
            }

            var threadRequest = new CreateThreadRequest(messages);
            await assistant.CreateThreadAndRunAsync(threadRequest, StreamEventHandler);
        }


        keepDatingWasCalled = false;
    }

    private void KeepDating()
    {
        var conversationEndNote = "Try to lead the conversation to an end to continue the date.  \r\n";

        string instructions = PrepareInstructions(conversationEndNote, topics);

        assistant.ModifyAsync(new CreateAssistantRequest(instructions: instructions));

        keepDatingWasCalled = true;
    }

    private string PrepareInstructions(string conversationEndNote, string topics)
    {
        return $"You are on a date in a museum with the user. You are currently taking a break and chitchatting about stuff, primarily about yourself. \r\n" +
                    $"{conversationEndNote}\r\n" +
                    $"Speak in {language}. \r\n\r\n" +
                    $"The following things might be worth talking about:\r\n" +
                    $"(formated as: name - your thoughts)\r\n\r\n" +
                    $"{topics}" +
                    $"Don\"t talk about everthing at once. Mention it bit by bit as you lead the conversation. ";
    }

    private string PrepareTopics(Dictionary<string, int> topicPriorities)
    {
        var topics = "";
        foreach (Topic topic in this.topicsSO.topics)
        {
            if (topicPriorities.ContainsKey(topic.Name))
            {
                topics += $"Name: {topic.Name}\r\nState: {((topicPriorities[topic.Name] == 0) ? "Not yet visited" : "Already visited")}\r\nPriority: {topicPriorities[topic.Name]}\r\nThoughts: {topic.Thoughts}\r\n\r\n";
            }
        }

        return topics;
    }

    private void SendButton_clicked()
    {
        messages.Add(input.text);
        Send(messages);
    }

    async void Send(List<Message> messages)
    {   
        ThreadResponse thread = null;
         
        async Task StreamEventHandler(IServerSentEvent streamEvent)
        {
            switch (streamEvent)
            {
                case ThreadResponse threadResponse:
                    thread = threadResponse;
                    break;
                case RunResponse runResponse:
                    if (runResponse.Status == RunStatus.RequiresAction)
                    {
                        var toolOutputs = await assistant.GetToolOutputsAsync(runResponse);

                        foreach (var toolOutput in toolOutputs)
                        {
                            Debug.Log($"Tool Output: {toolOutput}");
                        }

                        await runResponse.SubmitToolOutputsAsync(toolOutputs, StreamEventHandler);
                    }
                    break;
                default:
                    Debug.Log("Unhandled:\n" + streamEvent.ToJsonString());
                    break;
            }

            var responseMessages = await thread.ListMessagesAsync();
            List<Message> messages = new List<Message>();

            foreach (var response in responseMessages.Items.Reverse())
            {
                messages.Add(new Message(response.PrintContent(), response.Role));
            }

            OnResponse(messages);
        }

        async Task StreamEventHandlerYesNo(IServerSentEvent streamEvent)
        {
            switch (streamEvent)
            {
                case ThreadResponse threadResponse:
                    thread = threadResponse;
                    break;
                case RunResponse runResponse:
                    if (runResponse.Status == RunStatus.RequiresAction)
                    {
                        var toolOutputs = await assistant.GetToolOutputsAsync(runResponse);

                        foreach (var toolOutput in toolOutputs)
                        {
                            Debug.Log($"Tool Output: {toolOutput}");
                        }

                        await runResponse.SubmitToolOutputsAsync(toolOutputs, StreamEventHandler);
                    }
                    break;
                default:
                    Debug.Log("Unhandled:\n" + streamEvent.ToJsonString());
                    break;
            }

            var responseMessages = await thread.ListMessagesAsync();
            if (responseMessages.Items[0].PrintContent().ToLower().Contains("yes")) OnContinueDate();
        }

        if (messages.Count > keepDatingCount && !keepDatingWasCalled) KeepDating();
        var threadRequest = new CreateThreadRequest(messages);
        await interpreter.CreateThreadAndRunAsync(threadRequest, StreamEventHandlerYesNo);
        await assistant.CreateThreadAndRunAsync(threadRequest, StreamEventHandler);

    }

    void TestHandleResponse(List<Message> messages)
    {
        Debug.LogWarning("Start");

        scrollView.contentContainer.Clear();
        foreach (var response in messages)
        {
            scrollView.contentContainer.Add(new Label($"{response.Role}:\n{response.PrintContent()}"));
            Debug.Log($"{response.Role}: {response.PrintContent()}");
        }

        scrollView.ScrollTo(scrollView.contentContainer.ElementAt(scrollView.contentContainer.childCount - 1));
    }
}

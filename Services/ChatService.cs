using MessangerBack.Models;
using MessangerBack.Repositories;
using MessangerBack.Exceptions;
using MessangerBack.Responces;


namespace MessangerBack.Services;

public class ChatService : IChatService
{
    IChatRepository _repository;
    IMessageService _messageService;

    public ChatService(IChatRepository repository, IMessageService messageService)
    {
        _repository = repository;
        _messageService = messageService;
    }

    public async Task<List<AllChatsResponce>> AllChatsByChatName(string chatName)
    {
        var chats = await _repository.AllChatsFilterByChatName(chatName);

        return ChatModelToAllChatsResponce(chats);
    }

    public async Task<ChatInfoResponce> CreateChat(Guid adminId, string chatName)
    {
        Guid[] users = { adminId };

        ChatModel chat = new()
        {
            Id = Guid.NewGuid(),
            AdminId = adminId, 
            Users = users,
            ChatName = chatName,
        };

        await _repository.CreateChat(chat);
        await _messageService.CreateMessage(adminId, chat.Id, "Создал чат");

        return new ChatInfoResponce()
        {
            Id = chat.Id,
            ChatName = chat.ChatName,
            AdminId = chat.AdminId,
            Admin = new()
            {
                Id = chat.Admin.Id,
                UserName = chat.Admin.UserName,
            },
            Users = chat.Users,
            LastMessageId = chat.LastMessageId,
            LastMessage = new()
            {
                Id = chat.LastMessage.Id,
                SenderId = chat.LastMessage.SenderId,
                Sender = new ()
                {
                    Id = chat.LastMessage.Sender.Id,
                    UserName = chat.LastMessage.Sender.UserName,
                },
                Text = chat.LastMessage.Text,
                MessageSentTime = chat.LastMessage.MessageSentTime
            }
        };
    }

    public async Task AddToChat(Guid userId, Guid chatId)
    {
        ChatModel chat;
        try
        {
            chat = await _repository.GetChatById(chatId);
        }
        catch (InvalidOperationException)
        {
            throw new NotFoundException("Чат не найден.");
        }
        
        Guid[] chatMembers = chat.Users;

        if (chatMembers.Contains(userId))
        {
            throw new WrongUserInputException("Пользователь состоит в чате.");
        }

        chatMembers = chatMembers.Concat(new Guid[] { userId }).ToArray();

        chat.Users = chatMembers;
        await _repository.UpdateChat(chat);
    }

    public async Task<List<AllChatsResponce>> GetAllUserChats(Guid userId)
    {
        List<AllChatsResponce> responce = new();
        
        List<ChatModel> chats = await _repository.GetAllUserChats(userId);
        foreach(var chat in chats)
        {
            responce.Add(
                new()
                {
                    Id = chat.Id,
                    ChatName = chat.ChatName,
                    LastMessageId = chat.LastMessageId,
                    LastMessage = new()
                    {
                        Id = chat.LastMessage.Id,
                        SenderId = chat.LastMessage.SenderId,
                        Sender = new ()
                        {
                            Id = chat.LastMessage.Sender.Id,
                            UserName = chat.LastMessage.Sender.UserName,
                        },
                        Text = chat.LastMessage.Text,
                        MessageSentTime = chat.LastMessage.MessageSentTime
                    }
                }
            );
        }

        return responce;
    }

    public async Task<ChatInfoResponce> GetChatInfo(Guid chatId)
    {
        ChatModel chat = await _repository.GetChatById(chatId);

        return new ChatInfoResponce()
        {
            Id = chat.Id,
            ChatName = chat.ChatName,
            AdminId = chat.AdminId,
            Admin = new()
            {
                Id = chat.Admin.Id,
                UserName = chat.Admin.UserName,
            },
            Users = chat.Users,
            LastMessageId = chat.LastMessageId,
            LastMessage = new()
            {
                Id = chat.LastMessage.Id,
                SenderId = chat.LastMessage.SenderId,
                Sender = new ()
                {
                    Id = chat.LastMessage.Sender.Id,
                    UserName = chat.LastMessage.Sender.UserName,
                },
                Text = chat.LastMessage.Text,
                MessageSentTime = chat.LastMessage.MessageSentTime
            }
        };
    }

    public async Task RemoveFromChat(Guid adminId, Guid userId, Guid chatId)
    {
        ChatModel chat;
        try
        {
            chat = await _repository.GetChatById(chatId);
        } 
        catch (InvalidOperationException)
        {
            throw new NotFoundException("Чат не найден.");
        }

        if (chat.AdminId != adminId)
        {
            throw new Forbidden("Удалять пользователей из чата может только админ.");
        }
        else if (adminId == userId)
        {
            throw new WrongUserInputException("Админ не может удалить самого себя из чата.");
        }

        List<Guid> users = new List<Guid>(chat.Users);
        users.Remove(userId);
        chat.Users = users.ToArray();

        await _repository.UpdateChat(chat);
    }

    private List<AllChatsResponce> ChatModelToAllChatsResponce(List<ChatModel> chats)
    {
        List<AllChatsResponce> responceChats = new();
        
        foreach(var chat in chats)
        {
            responceChats.Add(
                new()
                {
                    Id = chat.Id,
                    ChatName = chat.ChatName,
                    LastMessageId = chat.LastMessageId,
                    LastMessage = new()
                    {
                        Id = chat.LastMessage.Id,
                        SenderId = chat.LastMessage.SenderId,
                        Sender = new ()
                        {
                            Id = chat.LastMessage.Sender.Id,
                            UserName = chat.LastMessage.Sender.UserName,
                        },
                        Text = chat.LastMessage.Text,
                        MessageSentTime = chat.LastMessage.MessageSentTime
                    }
                }
            );
        }

        return responceChats;
    }
}

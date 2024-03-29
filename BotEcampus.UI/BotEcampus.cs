﻿using BotEcampus.Core.Models;
using BotEcampus.UI.DataBase;
using BotEcampus.UI.Services.BotCommand;
using EcampusApi.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VkApi.Group;
using VkApi.Group.Interfaces;
using VkNet.Enums.SafetyEnums;
using VkNet.Model.Keyboard;

namespace BotEcampus.Core
{
    public class BotEcampus
    {
        private IVKMessageManager messageManager;
        private IDataBase db;
        private readonly string groupId;
        public BotEcampus(string accessToken, ulong groupId, IDataBase dataBase)
        {
            if (!string.IsNullOrWhiteSpace(accessToken) && !string.IsNullOrEmpty(accessToken))
            {
                this.groupId = groupId.ToString();
                messageManager = new VKMessageManager(accessToken, groupId);
                messageManager.StartMessageHandling();
            }
            db = dataBase;
        }

       
        public void RunVkBot()
        {
            messageManager.OnNewMessage += async (message) =>
            {
                var userId = message.FromId;
                var fdd = message.Payload;
                var text = message.Text;
                
                var sheduleKeyboard = Commands.GetOptionalKeyboard();
                EcampusClient clientECampus = new EcampusClient();
                var isExist = db.GetUserById(userId);
                try
                {
                    if (isExist == null && !text.Contains(":") && text.Split(':').Length != 2)
                    {
                        await messageManager.SendMessageAsync("Здравствуйте, я Ваш помощник ECampusBot &#129302;\n" +
                               "Для дальнейшей работы со мной, Вам нужно авторизоваться в \"Электронный кампус СКФУ\"&#128104;&#8205;&#128187;\n" +
                               "Пример: login:password", userId);
                        return;
                    }
                    if (text.ToLower() == "начать")
                    {
                        if (isExist == null)
                        {
                            await messageManager.SendMessageAsync("Здравствуйте, я Ваш помощник ECampusBot &#129302;\n" +
                               "Для дальнейшей работы со мной, Вам нужно авторизоваться в \"Электронный кампус СКФУ\"&#128104;&#8205;&#128187;\n" +
                               "Пример: login:password", userId);
                            return;
                        }
                        else
                        {
                            await messageManager.SendMessageAsync("Функционал бота", userId);
                        }
                    }
                }
                catch (Exception ex)
                {
                    await messageManager.SendMessageAsync("Упс! Что-то пошло не так &#129301;\n" +
                        "Мы найдем и исправим эту ошибку &#128373; \n" + ex.Message, userId);
                }



                // ДЛЯ АВТОРИЗОВАННЫХ ЮЗЕРОВ
                try
                {
                    if (isExist != null)
                    {
                        var login = await clientECampus.LoginAsync(isExist.Split(':')[0], isExist.Split(':')[1]);

                        if (login.IsSuccess)
                        {
                            //TODO
                            //Ответы бота должны иметь класс, где мы можем увидеть тип ответа(txt,img,video and etc)
                            string answer = await Commands.GetAnswerForAuthorizeUser(text, clientECampus);
                            if (answer.Contains(".jpg"))
                            {
                                await messageManager.SendMessagePhotoAsync(answer,userId);
                                return;
                            }
                            await messageManager.SendMessageAsync(answer, userId, sheduleKeyboard);
                            return;
                        }
                        else
                        {
                            await messageManager.SendMessageAsync("Сервер вернул ошибку", userId, sheduleKeyboard);
                        }
                    }
                    else
                    {

                        var answer = await Commands.GetAnswerAnonymousUser(text, clientECampus);
                        if (answer.Contains("успешно"))
                        {
                            var login = text.Split(':')[0];
                            var password = text.Split(':')[1];
                            var resultAddingUser = db.AddUser(new Authorization
                            {
                                Login = login,
                                Password = password,
                                UserId = userId
                            });
                            await messageManager.SendMessageAsync(answer, userId, sheduleKeyboard);
                            return;
                        }
                        await messageManager.SendMessageAsync(answer, userId);
                    }
                }
                catch (Exception ex)
                {
                    await messageManager.SendMessageAsync("Упс! Что-то пошло не так &#129301;\n" +
                        "Мы найдем и исправим эту ошибку &#128373; \n" + ex.Message, userId);
                }

                
            };

        }
    }
}

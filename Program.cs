using Newtonsoft.Json;
using System;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using OpenQA.Selenium.Support.UI;
using Microsoft.VisualBasic;
using System.Collections;

namespace MudaeBOT
{
    class Program
    {
        static void Main()
        {

            #region Global Vars ================================================================

            #region Standard Vars ==============================================================

            // ================================
            string discordUrl = InternKeys.discordUrl;
            string serverid = InternKeys.serverid;
            string channelid = InternKeys.mainChannelid;
            string rollChannel = $"{discordUrl}/{serverid}/{channelid}";
            // ================================
            string[] diagChannel = new string[]
            {
                "1234334387450155009",
                "1234334427333660713",
                "1234334484229263471"
            };
            // ================================
            DiscordAcc[][] list = AccKeys.list;
            // ================================
            bool turnFarmUS = false;
            bool doLogging = false;
            bool KakeraCriteria = true;
            // ================================
            int minimunKakera = 100;
            int kpUsage = 36;
            // ================================
            IWebElement div_scroller;
            IReadOnlyList<IWebElement> message_list;
            //IReadOnlyList<IWebElement> channel_list;
            IReadOnlyCollection<IWebElement> MessageContentHolderElementList;
            IWebElement? MessageContentHolderElement;
            IWebElement last_message;
            // ================================
            Character lastChar = new Character { name = null, buttonList = null, messageId = null };
            Character charMarryTarget = new Character { name = "null", buttonList = null, messageId = "0" };
            string[] MessageContentHolder;
            string last_message_id;
            string Character;
            string Serie;
            string Kakera;
            string ButtomTypeText;
            string EmbFooterInfo;
            string fullEmbDesc;
            string kakeraEarnedOnReaction = "0";
            // ================================
            bool lastMsgSucces;
            bool charMarriedStatus;
            bool new_message = true;
            bool isEmbbed;
            bool isCharacter;
            bool lessThan3Min = false;
            // ================================
            string? messageIdHolder = "0";
            string messageOwnerName;
            string messageTimeStamp;
            string messageText;
            // ================================
            string[] allowedKakeraButtons = new string[] { "P", "O", "R", "W", "L" };
            // ================================
            #endregion

            #region Control Vars - Setup inicialization ========================================

            bool writeToWebBrowser = true;

            bool finalMode = false;
            bool singprof = false;
            bool singleAcc = false;
            bool configMode = true;
            bool asyncTasks = false;
            bool master = true;
            bool doDiagAtStart = true;

            int accounts = 5;
            int instancesNumber = 3;
            int cprofile = 0;

            bool oneToOne = true;
            if (oneToOne) { accounts = 1; instancesNumber = 1; }

            int groupacc = cprofile;
            if (singprof) { instancesNumber = 1; }
            if (singleAcc) { singprof = true; accounts = 1; }
            if (finalMode) { singprof = false; singleAcc = false; configMode = false; asyncTasks = false; }

            string chromeDriverPath = @"C:\Users\ale_5\Desktop\Projects\CS-MudaeBot\chromedriver.exe";
            string profileDirectory = @"C:\Users\ale_5\Desktop\Projects\CS-MudaeBot\chromeprofiles\";

            NavigatorInstance[] chatBot;
            chatBot = new NavigatorInstance[instancesNumber];

            // ================================

            ChromeOptions options = new ChromeOptions();
            options.AddArgument($@"--user-data-dir={profileDirectory}\log");
            ChromeDriver logBrowser = new ChromeDriver(chromeDriverPath, options);
            logBrowser.Navigate().GoToUrl($"{InternKeys.webLogPath}");

            NavigatorInitializer(chromeProfile: cprofile, instances: instancesNumber, singProf: singprof, accountNumber: accounts, asyncron: asyncTasks).Wait();

            ChromeOptions options2 = new ChromeOptions();
            options2.AddArgument($@"--user-data-dir={profileDirectory}\master");
            NavigatorInstance[] masterAcc = new NavigatorInstance[1];
            masterAcc[0] = new NavigatorInstance { driver = new ChromeDriver(chromeDriverPath, options2) };
            masterAcc[0].actions = new Actions(masterAcc[0].driver);
            masterAcc[0].accounts = new DiscordAcc[1];
            masterAcc[0].windowHandler = masterAcc[0].driver.WindowHandles;
            masterAcc[0].accounts = new DiscordAcc[1];
            masterAcc[0].accounts[0] = new DiscordAcc
            {
                isActive = true,
                navHost = masterAcc[0],
                driverTab = 0,
                Id = InternKeys.masterId,
                Nick = "Nie",
                Discriminator = $"<@{InternKeys.masterId}>",
                navHostIndex = 0
            };
            RandomSleep(1.0, 2.5);
            masterAcc[0].driver.SwitchTo().Window(masterAcc[0].windowHandler[0]);
            ((IJavaScriptExecutor)masterAcc[0].driver).ExecuteScript("window.focus();");
            if (masterAcc[0].driver.Url != rollChannel) { masterAcc[0].driver.Navigate().GoToUrl(rollChannel); }
            masterAcc[0].driver.Navigate().GoToUrl(rollChannel);
            LoadElement(ref masterAcc[0]);
            if (ClickBottomChat(ref masterAcc[0])) RandomSleep(3.5, 4.5);

            #endregion

            #endregion

            // =================================================================================

            #region Initialization Functions ===================================================

            async Task NavigatorInitializer(int chromeProfile, int instances, int accountNumber, bool singProf = false, bool asyncron = false)
            {
                if (asyncron)
                {
                    Task[] tasks = new Task[instances];

                    Console.WriteLine($"Working with {instances} async instances");
                    for (int i = 0; i < instances; i++)
                    {
                        int navIndex = i;
                        tasks[navIndex] = Task.Run(async () =>
                        {
                            WriteToConsole($"starting {i} instance");
                            await awaitedNavInit(singProf, chromeProfile, navIndex, accountNumber);
                        });
                    }
                    await Task.WhenAll(tasks);
                }
                else
                {
                    for (int i = 0; i < instances; i++)
                    {
                        Console.WriteLine($"Working with {instances} async instances");
                        int navIndex = i;
                        Console.WriteLine($"starting {i} instance");
                        await awaitedNavInit(singProf, chromeProfile, navIndex, accountNumber);
                    }
                }

                Console.WriteLine("Account report:");
                for (int i = 0; i < chatBot.Length; i++)
                {
                    for (int j = 0; j < chatBot[i].accounts.Length; j++)
                    {
                        Console.WriteLine($"{chatBot[i].accounts[j].Nick}:  {chatBot[i].accounts[j].marriable} {chatBot[i].accounts[j].kakeraPower}");
                    }
                }
            }

            async Task awaitedNavInit(bool singProf, int chromeProfile, int navIndexer, int accountNumber)
            {
                bool oneProfile = singProf;
                ChromeOptions options = new ChromeOptions();
                if (oneProfile) options.AddArgument($@"--user-data-dir={profileDirectory}\{chromeProfile}");
                else options.AddArgument($@"--user-data-dir={profileDirectory}\{navIndexer}");
                chatBot[navIndexer] = new NavigatorInstance { driver = new ChromeDriver(chromeDriverPath, options) };
                chatBot[navIndexer].actions = new Actions(chatBot[navIndexer].driver);
                chatBot[navIndexer].accounts = list[navIndexer];
                chatBot[navIndexer].ownIndex = navIndexer;

                if (!oneProfile)
                {
                    for (int j = 1; j < accountNumber; j++) { ((IJavaScriptExecutor)chatBot[navIndexer].driver).ExecuteScript("window.open();"); }
                    chatBot[navIndexer].windowHandler = chatBot[navIndexer].driver.WindowHandles;
                }
                else
                {
                    chatBot[navIndexer].windowHandler = chatBot[0].driver.WindowHandles;
                }

                for (int k = 0; k < accountNumber; k++)
                {
                    int accIndexer = k;
                    Console.WriteLine($"instance {navIndexer}, Account {k} ");
                    AccountInitializer(ref chatBot[navIndexer].accounts, accIndexer, navIndexer, ref chatBot[navIndexer]);
                }
            }

            void AccountInitializer(ref DiscordAcc[] accounts, int accIndexer, int navIndexer, ref NavigatorInstance nav)
            {
                accounts[navIndexer].navHostIndex = navIndexer;
                accounts[navIndexer].navHost = chatBot[navIndexer];
                accounts[accIndexer].isActive = true;
                WriteToConsole($"Initializing {accounts[accIndexer].Nick}");
                nav.driver.SwitchTo().Window(nav.windowHandler[accIndexer]);
                ((IJavaScriptExecutor)nav.driver).ExecuteScript("window.focus();");
                if (nav.driver.Url != rollChannel)
                {
                    nav.driver.Navigate().GoToUrl(rollChannel);
                    RandomSleep(2.0, 2.5);
                    LoadElement(ref nav);
                }
                accounts[accIndexer].driverTab = accIndexer;
                SwitchAcc(ref nav, accounts[accIndexer].Id);
                if (doDiagAtStart) { SendDiagn(nav: ref nav, acc: ref accounts[accIndexer], doOnDiagChannel: false); }
            }

            #endregion

            // =================================================================================

            #region Master Functions ===================================================

            void RollLoop(ref NavigatorInstance masterAcc)
            {
                if (lessThan3Min)
                {
                    lessThan3Min = false;
                    Thread.Sleep(1000 * 60 * 3);
                    RollLoop(ref masterAcc);
                }
                NavigatorInstance reader = chatBot[0];
                StartRolling(ref masterAcc);
                SendUs(ref masterAcc, true);
            }

            void SendUs(ref NavigatorInstance masterAcc, bool infinite = false)
            {
                NavigatorInstance reader = chatBot[0];
                int rollNumber = 20;
                SendMessage("$us 20", ref masterAcc, ref masterAcc.accounts[0], noDelay: true);
                RandomSleep(0.3, 0.6);
                while (rollNumber > 0)
                {
                    if (RollReturnBool(ref masterAcc, ref reader, "$wa")) 
                    {
                        RandomSleep(0.3, 0.7);
                        ReadLastMsgId(ref reader, completeInfo: true); 
                        EventTrigger(targetId: last_message_id, instance: ref chatBot); 
                        rollNumber--; 
                    }
                }
                CheckKu(ref masterAcc, masterAcc.accounts[0].Id);
                if (lessThan3Min) return;
                if (infinite) SendUs(ref masterAcc, true);
            }

            void CountRolls(ref NavigatorInstance masterAcc)
            {
                NavigatorInstance reader = chatBot[0];
                SendMessage("$ru", ref masterAcc, ref masterAcc.accounts[0], noDelay: true);
                RandomSleep(2.1, 2.2);
                masterAcc.accounts[0].rolls = int.Parse($@"{RetrieveLastMessageText(ref reader)}".Split(" ")[2]);
            }

            void StartRolling(ref NavigatorInstance masterAcc, bool reCall = false)
            {
                NavigatorInstance reader = chatBot[0];

                if (!reCall) CountRolls(ref masterAcc);
                int rollNumber = masterAcc.accounts[0].rolls;
                while (rollNumber > 0)
                {
                    if (RollReturnBool(ref masterAcc, ref reader, "$wa")) 
                    {
                        RandomSleep(0.3, 0.7);
                        ReadLastMsgId(ref reader, completeInfo: true); 
                        EventTrigger(targetId: last_message_id, instance: ref chatBot); 
                        rollNumber--; 
                    }
                }
                CountRolls(ref masterAcc);
                if (masterAcc.accounts[0].rolls > 0) { StartRolling(ref masterAcc, reCall: true); }
            }

            bool RollReturnBool(ref NavigatorInstance nav, ref NavigatorInstance reader, string rollType)
            {
                SendMessage(rollType, ref nav, ref nav.accounts[0], false);
                IWebElement messageElement;
                RandomSleep(0.2, 0.4);
                for (int i = 0; i < 3; i++)
                {
                    if (!ExpectMessage(ref reader, rollType, out messageElement))
                    {
                        return true;
                    }
                    RandomSleep(1.3, 1.5);
                    SendMessage(rollType, ref nav, ref nav.accounts[0], false);
                }
                return false;
            }

            IWebElement RollReturnElement(ref NavigatorInstance nav, ref NavigatorInstance reader, string rollType)
            {
                SendMessage(rollType, ref nav, ref nav.accounts[0], false);
                IWebElement messageElement;
                for (int i = 0; i < 3; i++)
                {
                    if (ExpectMessage(ref reader, rollType, out messageElement))
                    {
                        return messageElement;
                    }
                    RandomSleep(2.7, 3.5);
                    SendMessage(rollType, ref nav, ref nav.accounts[0], false);
                }
                return null;
            }

            #endregion

            // =================================================================================

            #region Functions ==================================================================

            #region LoadElement ( string path, int limitTry, int interval, string SelectorMethod, bool byDrive, bool runAll, IWebDriver driver, SelPath? PathList, IWebElement Element )
            IWebElement LoadElement(
            ref NavigatorInstance nav,
            string path = "body",
            int limitTry = 15,
            int interval = 1,
            string SelectorMethod = "css",
            bool byDrive = false,
            bool runAll = false,
            SelPath? PathList = null,
            IWebElement? Element = null
            )
            {
                if (ClickBottomChat(ref nav)) RandomSleep(3.5, 4.5);
                if (PathList is null) { runAll = false; }
                if (path == "body") { SelectorMethod = "tagName"; byDrive = true; }
                int tryBeforeSucess = 1;
                IWebElement? ElementResult = null;
                for (int i = 0; i < limitTry; i++)
                {
                    try
                    {
                        if (!byDrive)
                        {
                            if (runAll)
                            {
                                if (PathList.xpath is not null) { ElementResult = Element.FindElement(By.XPath(PathList.xpath)); if (ElementResult is not null) return ElementResult; }
                                if (PathList.tagName is not null) { ElementResult = Element.FindElement(By.TagName(PathList.tagName)); if (ElementResult is not null) return ElementResult; }
                                if (PathList.tagClass is not null) { ElementResult = Element.FindElement(By.ClassName(PathList.tagClass)); if (ElementResult is not null) return ElementResult; }
                                if (PathList.cssSelector is not null) { ElementResult = Element.FindElement(By.CssSelector(PathList.cssSelector)); if (ElementResult is not null) return ElementResult; }
                            }
                            else
                            {
                                switch (SelectorMethod)
                                {
                                    case "xpath": ElementResult = Element.FindElement(By.XPath(path)); break;
                                    case "tagName": ElementResult = Element.FindElement(By.TagName(path)); break;
                                    case "className": ElementResult = Element.FindElement(By.ClassName(path)); break;
                                    default: ElementResult = Element.FindElement(By.CssSelector(path)); break;
                                }
                            }
                        }
                        else
                        {
                            if (runAll)
                            {
                                if (PathList.xpath is not null) { ElementResult = nav.driver.FindElement(By.XPath(PathList.xpath)); if (ElementResult is not null) return ElementResult; }
                                if (PathList.tagName is not null) { ElementResult = nav.driver.FindElement(By.TagName(PathList.tagName)); if (ElementResult is not null) return ElementResult; }
                                if (PathList.tagClass is not null) { ElementResult = nav.driver.FindElement(By.ClassName(PathList.tagClass)); if (ElementResult is not null) return ElementResult; }
                                if (PathList.cssSelector is not null) { ElementResult = nav.driver.FindElement(By.CssSelector(PathList.cssSelector)); if (ElementResult is not null) return ElementResult; }
                            }
                            else
                            {
                                switch (SelectorMethod)
                                {
                                    case "xpath": ElementResult = nav.driver.FindElement(By.XPath(path)); break;
                                    case "tagName": ElementResult = nav.driver.FindElement(By.TagName(path)); break;
                                    case "className": ElementResult = nav.driver.FindElement(By.ClassName(path)); break;
                                    default: ElementResult = nav.driver.FindElement(By.CssSelector(path)); break;
                                }
                            }
                        }
                        Thread.Sleep(1000);
                        return ElementResult;
                    }
                    catch { tryBeforeSucess++; }
                    Thread.Sleep(interval * 1000);
                }
                return null;
            }
            #endregion

            void RandomSleep(double higherval, double lowerval)
            {
                Random rnd = new Random();
                if (higherval < lowerval) (higherval, lowerval) = (lowerval, higherval);
                Thread.Sleep((int)(((rnd.NextDouble() * (higherval - lowerval)) + lowerval) * 1000));
            } // A random sleep between the inserted intervals.

            bool ClickBottomChat(ref NavigatorInstance nav)
            {
                bool foundBottomChatButton;
                try
                {
                    IWebElement chatdownButton = nav.driver.FindElement(By.CssSelector(SelPaths.BottonScrollerButton.cssSelector));
                    chatdownButton.Click();
                    foundBottomChatButton = true;
                    RandomSleep(1.0, 1.2);
                }
                catch
                {
                    foundBottomChatButton = false;
                }
                return foundBottomChatButton;
            } // Goes to the bottom of the chat

            string NavActualAcc(ref NavigatorInstance nav)
            {
                try
                {
                    IWebElement imgElement = nav.driver.FindElement(By.ClassName("avatarStack__789b4"));
                    return imgElement.FindElement(By.TagName("img")).GetAttribute("src").Split("/")[4];
                }
                catch
                {
                    return "0";
                }
            } // Return the ID of actual account in the Nav

            int[] AccountIndexFinder(string id)
            {
                // list [group] [account] . Id / Nick/ Discriminator
                for (int i = 0; i < list.Length; i++)
                {
                    for (int j = 0; j < list[i].Length; j++)
                    {
                        if (list[i][j].Id == id) { return new int[] { i, j }; }
                    }
                }
                return new int[] { 0, 0 };
            } // Return int[] with 0 being Nav, 1 being Account. Based on ID

            // ================================

            bool ExpectMessage(ref NavigatorInstance nav, string messageComparator, out IWebElement messageElement, int limit = 100, int recallNumber = 1)
            {
                for (int x = 0; x < recallNumber; x++)
                {
                    int i = 0;
                    while (i < limit)
                    {
                        try 
                        {
                            div_scroller = nav.driver.FindElement(By.CssSelector(SelPaths.scrollerBox.cssSelector));
                            IReadOnlyCollection<IWebElement> MessageList = div_scroller.FindElements(By.TagName("li"));
                            IWebElement lastMessage = MessageList.Last();
                            string lastMessageId = lastMessage.GetAttribute("id").Split('-')[^1];

                            if (messageIdHolder != lastMessageId)
                            {
                                //  #chat-messages-1112789095194837022-1241605180898676806 > div > div.contents_d3ae0d
                                string messageText = lastMessage.FindElement(By.CssSelector($"#chat-messages-{channelid}-{lastMessageId} > div > div.contents_d3ae0d")).Text;
                                if (messageComparator != messageText)
                                {
                                    messageElement = lastMessage;
                                    lastMsgSucces = true;
                                    return false;
                                }
                            }
                            Thread.Sleep(20);
                            i++;
                        }
                        catch { i++; }
                        
                    }
                    messageElement = null;
                    lastMsgSucces = true;
                }
                messageElement = null;
                return true;
            } // Checks if the last message has a different id from last msgIdHolder AND is different from a specific text

            IWebElement LastMessageElement(ref NavigatorInstance nav)
            {
                if (ClickBottomChat(ref nav)) RandomSleep(3.5, 4.5);
                try
                {
                    IWebElement divScroller = nav.driver.FindElement(By.CssSelector(SelPaths.scrollerBox.cssSelector));
                    IReadOnlyList<IWebElement> messageList = divScroller.FindElements(By.TagName("li"));
                    return messageList[messageList.Count - 1];
                }
                catch
                {
                    return last_message;
                }
            } // Return a IWebElement referant to the last message on discord.

            string RetrieveLastMessageText(ref NavigatorInstance nav)
            {
                IWebElement messageElement = LastMessageElement(ref nav);
                string messageId = messageElement.GetAttribute("id").Split('-')[^1];
                return messageElement.FindElement(By.CssSelector($"#message-content-{messageId}")).Text;
            }

            bool TryRetrieveMessageElementById(ref NavigatorInstance nav, string messageTargetId, out IWebElement messageElement)
            {
                if (ClickBottomChat(ref nav)) RandomSleep(3.5, 4.5);

                IWebElement divScroller = nav.driver.FindElement(By.CssSelector(SelPaths.scrollerBox.cssSelector));
                IReadOnlyList<IWebElement> messageList = divScroller.FindElements(By.TagName("li"));
                foreach (IWebElement message in messageList)
                {
                    string iteratedMessageId = RetrieveMessageId(message);
                    if (messageTargetId == iteratedMessageId)
                    {
                        messageElement = message;
                        return true;
                    }
                }
                messageElement = null; return false;

            } // Try to return a message on discord, based on messageID

            string RetrieveMessageId(IWebElement message)
            {
                try
                {
                    string id = message.GetAttribute("id").Split('-')[^1];
                    return id;
                }
                catch { return "0"; }

            } // Return the message ID, based on IWebElement

            void ReadLastMsgId(ref NavigatorInstance nav, bool completeInfo = false, bool ReactToRoll = false)
            {
                if (ClickBottomChat(ref nav)) RandomSleep(3.5, 4.5);
                try
                {
                    div_scroller = nav.driver.FindElement(By.CssSelector(SelPaths.scrollerBox.cssSelector));
                    message_list = div_scroller.FindElements(By.TagName("li"));
                    last_message = message_list[message_list.Count - 1];
                    last_message_id = last_message.GetAttribute("id");
                    last_message_id = last_message_id.Split('-')[^1];
                    if (messageIdHolder != last_message_id) { new_message = true; messageIdHolder = last_message_id; }
                    else new_message = false;
                    lastMsgSucces = true;

                }
                catch
                {
                    WriteToConsole("Err Identify");
                    last_message_id = "0";
                    last_message = null;
                    lastMsgSucces = false;
                    new_message = true;
                }

                if (completeInfo && (lastMsgSucces && new_message)) ReadLastMessage(ref nav);
            } // Retrieve the lastMessageId. accessible from 'last_message_id'

            void ReadLastMessage(ref NavigatorInstance nav)
            {
                if (ClickBottomChat(ref nav)) RandomSleep(3.5, 4.5);
                // =================================================================================
                // Identify new messages

                try // Get span list
                {
                    MessageContentHolderElement = last_message.FindElement(By.CssSelector($"#chat-messages-{channelid}-{last_message_id} > div > div.contents_d3ae0d"));
                    string messageText = MessageContentHolderElement.Text;
                }
                catch
                {
                    MessageContentHolderElement = null;
                    MessageContentHolder = new string[] { "0" };
                }

                try // Get owner Name / timestamp
                {
                    // #chat-messages-1112789095194837022-1234247807246930163 > div > div.contents_d3ae0d > h3
                    IWebElement MessageInfoHolderElement = MessageContentHolderElement.FindElement(By.CssSelector($"#chat-messages-{channelid}-{last_message_id} > div > div.contents_d3ae0d > h3"));
                    IReadOnlyCollection<IWebElement> MessageInfoHolderElements = MessageInfoHolderElement.FindElements(By.TagName("span"));
                    string[] MessageInfoHolderString = new string[MessageInfoHolderElements.Count];
                    int i = 0;
                    messageTimeStamp = MessageInfoHolderElements.First().FindElement(By.TagName("time")).GetAttribute("datetime");
                    messageTimeStamp = string.Join(" ", messageTimeStamp.Split("T"));
                    messageOwnerName = MessageInfoHolderElements.Last().Text;
                }
                catch
                {
                    messageTimeStamp = "notFound";
                    messageOwnerName = "notFound";
                }

                try // Get message text
                {
                    messageText = last_message.FindElement(By.CssSelector($"#message-content-{last_message_id}")).Text;
                    if (messageText[0..2].Contains("c$")) { UserCommand(messageText, ref nav); }

                }
                catch
                {
                    messageText = "notFound";
                }

                // =================================================================================
                // Character - Kakera
                try
                {
                    Character = last_message.FindElement(By.CssSelector($"{SelPaths.EmbbedAuthor.cssWithIdStart}{last_message_id}{SelPaths.EmbbedAuthor.cssWithIdEnd}")).Text;
                    if (Character.Contains("harem"))
                    {
                        isCharacter = false;
                    }
                    else
                    {
                        isCharacter = true;
                    }
                    if (isCharacter) lastChar.name = Character;
                    IWebElement EmbDesc = last_message.FindElement(By.CssSelector($"{SelPaths.EmbbedDesc.cssWithIdStart}{last_message_id}{SelPaths.EmbbedDesc.cssWithIdEnd}"));
                    fullEmbDesc = EmbDesc.Text;
                    Serie = ((fullEmbDesc).Split("Claims")[0]);
                    IReadOnlyCollection<IWebElement> EmbDescSpanList = EmbDesc.FindElements(By.TagName("span"));
                    Serie = (EmbDescSpanList.First()).Text;
                    IReadOnlyCollection<IWebElement> EmbDescStrongList = EmbDesc.FindElements(By.TagName("strong"));
                    Kakera = (EmbDescStrongList.Last()).Text;

                    if (isCharacter)
                    {
                        lastChar.value = int.Parse(Kakera);
                        lastChar.serie = Serie;
                        lastChar.messageId = last_message_id;
                    }

                    isEmbbed = true;
                }
                catch
                {
                    fullEmbDesc = "null";
                    Character = "null";
                    Kakera = "0";
                    Serie = "null";
                    isEmbbed = false;
                    isCharacter = false;
                }

                // =================================================================================
                // Button - Button Type
                string[] buttomListAppendChar;
                try
                {
                    IWebElement buttonsElementHolder = last_message.FindElement(By.XPath($"//*[@id=\"message-accessories-{last_message_id}\"]/div"));
                    buttonsElementHolder = buttonsElementHolder.FindElement(By.ClassName("children_f15443"));
                    IList<IWebElement> ButtomList = buttonsElementHolder.FindElements(By.TagName("button"));
                    buttomListAppendChar = new string[ButtomList.Count()];
                    int i = 0;
                    foreach (IWebElement button in ButtomList)
                    {
                        buttomListAppendChar[i] = $"{button.FindElement(By.TagName("img")).GetAttribute("alt")[^1]}"; i++;
                    }
                    if (isCharacter) lastChar.hasButton = true;
                }
                catch { buttomListAppendChar = null; if (isCharacter) lastChar.hasButton = false; }
                if (isCharacter) lastChar.buttonList = buttomListAppendChar;

                // =================================================================================
                // Character Status
                try
                {
                    IWebElement EmbFooterElement = last_message.FindElement(By.CssSelector($"{SelPaths.EmbbedFooter.cssWithIdStart}{last_message_id}{SelPaths.EmbbedFooter.cssWithIdEnd}"));
                    EmbFooterInfo = (EmbFooterElement.FindElement(By.TagName("span"))).Text;
                    if (EmbFooterInfo.Contains("Pertence")) charMarriedStatus = true;
                    else charMarriedStatus = false;
                }
                catch { EmbFooterInfo = "null"; charMarriedStatus = false; }
                if (isCharacter) lastChar.owned = charMarriedStatus;

                // =================================================================================

                if (doLogging)
                {
                    WriteToConsole($"MSG OwnerName: {messageOwnerName}");
                    WriteToConsole($"Time: {messageTimeStamp}");
                    if (isEmbbed)
                    {
                        if (isCharacter)
                        {
                            WriteToConsole($"MSG ID: {last_message_id}");
                            WriteToConsole($"Character: {Character}");
                            WriteToConsole($"Serie: {Serie}");
                            WriteToConsole($"Kakera Value: {Kakera}");
                            WriteToConsole($"Embbed Footer: {EmbFooterInfo}");
                            WriteToConsole($"Marryed: {charMarriedStatus}");
                        }

                    }
                    else
                    {
                        WriteToConsole($"MSG Text: {messageText}");
                    }
                }

            } // Retrieve the lastMessage. Then place the results in standard Vars

            void TryConvertMessageToChar(ref NavigatorInstance nav, string messageId, ref Character newCharacter)
            {
                string newCharname;
                string newSerie;
                string newKakera;
                bool isMarried;

                if (ClickBottomChat(ref nav)) RandomSleep(3.5, 4.5);

                IWebElement messageElement;
                TryRetrieveMessageElementById(ref nav, messageId, out messageElement);
                IWebElement embedElement = messageElement.FindElement(By.XPath($"//*[@id=\"message-accessories-{messageId}\"]/article/div/div"));
                try { newCharname = embedElement.FindElement(By.CssSelector($"{SelPaths.EmbbedAuthor.cssWithIdStart}{messageId}{SelPaths.EmbbedAuthor.cssWithIdEnd}")).Text; }
                catch { newCharname = "null"; }

                IWebElement EmbDesc = embedElement.FindElement(By.CssSelector($"{SelPaths.EmbbedDesc.cssWithIdStart}{messageId}{SelPaths.EmbbedDesc.cssWithIdEnd}"));

                try
                {
                    newSerie = ((EmbDesc.Text).Split("Claims")[0]);
                    IReadOnlyCollection<IWebElement> EmbDescSpanList = EmbDesc.FindElements(By.TagName("span"));
                    newSerie = (EmbDescSpanList.First()).Text;
                }
                catch { newSerie = "null"; }

                try
                {
                    IReadOnlyCollection<IWebElement> EmbDescStrongList = EmbDesc.FindElements(By.TagName("strong"));
                    newKakera = (EmbDescStrongList.Last()).Text;
                }
                catch { newKakera = "0"; }

                try
                {
                    IWebElement EmbFooterElement = messageElement.FindElement(By.XPath($"//*[@id=\"message-accessories-{messageId}\"]/article/div/div/div[3]"));
                    string EmbFooterText = EmbFooterElement.FindElement(By.ClassName("embedFooterText_c3068e")).Text;
                    if (EmbFooterText.Contains("Pertence")) isMarried = true;
                    else isMarried = false;
                }
                catch { isMarried = false; }

                newCharacter.name = newCharname;
                newCharacter.serie = newSerie;
                newCharacter.value = int.Parse(newKakera);
                newCharacter.owned = isMarried;
            } // Try to return a Character, based on a message

            // ================================

            void SwitchChannel(ref NavigatorInstance nav, string channelTarget)
            {
                IWebElement channelPanel = nav.driver.FindElement(By.CssSelector(SelPaths.ChannelList.cssSelector));
                IReadOnlyCollection<IWebElement> channelList = channelPanel.FindElements(By.TagName("li"));
                foreach (IWebElement channel in channelList)
                {
                    string channelName = channel.GetAttribute("data-dnd-name");
                    if (channelName == channelTarget)
                    {
                        ((IJavaScriptExecutor)nav.driver).ExecuteScript("window.focus();");
                        channel.Click(); RandomSleep(1.5, 1.7); break; 
                    }
                }
            } // Switch the current channel, based on channel id

            void SwitchAcc(ref NavigatorInstance nav, string targetId)
            {
                try
                {
                    string actualNavAcc = NavActualAcc(ref nav);
                    if (actualNavAcc != targetId)
                    {
                        IWebElement accountButton1 = nav.driver.FindElement(By.CssSelector(SelPaths.switchAcc1.cssSelector));
                        accountButton1.Click();
                        IWebElement accountButton2 = LoadElement(nav: ref nav, path: "#account-switch-account > div.label__563c3", SelectorMethod: "css", byDrive: true);
                        accountButton2.Click();
                        IWebElement accountButton3 = LoadElement(nav: ref nav, path: SelPaths.switchAcc2.cssSelector, SelectorMethod: "css", byDrive: true);
                        // IWebElement accountButton3 = driver.FindElement(By.CssSelector(SelPaths.switchAcc2.cssSelector));
                        IReadOnlyCollection<IWebElement> AccountListSwitch = accountButton3.FindElements(By.ClassName("accountCard__21066"));
                        string accountId = "0";
                        foreach (IWebElement accountinfo in AccountListSwitch)
                        {
                            try { accountId = accountinfo.FindElement(By.TagName("img")).GetAttribute("src"); accountId = accountId.Split("/")[4]; }
                            catch { accountId = "0"; }
                            if (accountId == targetId)
                            {
                                IReadOnlyCollection<IWebElement> AccountButtonsSwitch = accountinfo.FindElements(By.TagName("button"));
                                foreach (IWebElement button in AccountButtonsSwitch)
                                {
                                    try { if (button.FindElement(By.TagName("div")).Text == "Utilizar") { button.Click(); } }
                                    catch { }
                                }
                            }
                        }
                        if (nav.driver.Url != rollChannel) { nav.driver.Navigate().GoToUrl(rollChannel); }

                        LoadElement(ref nav);
                        Thread.Sleep(2000);
                        if (ClickBottomChat(ref nav)) RandomSleep(3.5, 4.5);
                    }
                }
                catch { Console.WriteLine("Switch Account Falhou"); }
            } // Switch the current Account, based on Account Id (discord switching proccess)

            void SwitchAccTabById(string targetId, ref NavigatorInstance nav)
            {
                int[] accIndex = AccountIndexFinder(targetId);
                nav.driver.SwitchTo().Window(nav.windowHandler[accIndex[1]]);
                ((IJavaScriptExecutor)nav.driver).ExecuteScript("window.focus();");
                if (ClickBottomChat(ref nav)) RandomSleep(3.5, 4.5);
                ((IJavaScriptExecutor)nav.driver).ExecuteScript("arguments[0].scrollIntoView(true);", LastMessageElement(ref nav));
            } // Switch the current Account, based on Account Id (tab switching proccess)

            void SwitchAccTabByIndex(ref NavigatorInstance nav, int[] index)
            {
                nav.driver.SwitchTo().Window(nav.windowHandler[index[1]]);
                ((IJavaScriptExecutor)nav.driver).ExecuteScript("window.focus();");
                if (ClickBottomChat(ref nav)) RandomSleep(3.5, 4.5);
                ((IJavaScriptExecutor)nav.driver).ExecuteScript("arguments[0].scrollIntoView(true);", LastMessageElement(ref nav));
            } // Switch the current Account, based on Account Id (tab switching proccess)

            // ================================

            void SendMarryLog(string charname, string ownerName, string result)
            {
                string script = @$"
                var ul = document.getElementById('leftMessages');
                var li = document.createElement('li');
                var ownerName = document.createElement('strong');
                ownerName.className = 'ownerName';
                ownerName.textContent = '{ownerName}';
                li.appendChild(ownerName);
                li.appendChild(document.createTextNode(' >> '));
                var charName = document.createElement('strong');
                charName.className = 'charName';
                charName.textContent = '{charname}';
                li.appendChild(charName);
                li.appendChild(document.createTextNode(' >> '));
                var status = document.createElement('strong');
                status.className = 'status-{result}';
                status.textContent = ' {result}';
                li.appendChild(status);
                ul.appendChild(li);
                ";
                IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)logBrowser;
                jsExecutor.ExecuteScript(script);
            } // Send the log to the web console

            void SendKakeraLog(string charName, string ownerName, bool result, string type, string amountEarned = "0")
            {
                try
                {
                    string boolResult;
                    string boolText;
                    if (result) { boolResult = "true"; boolText = amountEarned; }
                    else { boolResult = "false"; boolText = "Failure"; }

                    string script = $@"
                    var div = document.getElementById('rightMessages');
                    var li = document.createElement('li');

                    var ownerNameElement = document.createElement('strong');
                    ownerNameElement.className = 'ownerName';
                    ownerNameElement.textContent = '{ownerName}';
                    li.appendChild(ownerNameElement);

                    li.appendChild(document.createTextNode(' >> '));

                    var charNameElement = document.createElement('strong');
                    charNameElement.className = 'type{type}';
                    charNameElement.textContent = '{charName}';
                    li.appendChild(charNameElement);

                    li.appendChild(document.createTextNode(' >> '));

                    var statusElement = document.createElement('strong');
                    statusElement.className = 'status-{boolResult}';
                    statusElement.textContent = '{boolText}';
                    li.appendChild(statusElement);

                    div.appendChild(li);
                    ";
                    IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)logBrowser;
                    jsExecutor.ExecuteScript(script);
                }
                catch { }

            } // Send the log to the web console

            void SendDiagLog(string ownerName, bool marry, bool rt, int kp, int kakera)
            {
                string script = $@"
                var div = document.getElementById('bottomMessages');
                var li = document.createElement('li');

                li.appendChild(document.createTextNode('Diag: '));

                var ownerNameElement = document.createElement('strong');
                ownerNameElement.className = 'ownerName';
                ownerNameElement.textContent = '{ownerName}';
                li.appendChild(ownerNameElement);

                li.appendChild(document.createTextNode(' $'));

                var dollarElement = document.createElement('strong');
                dollarElement.textContent = '{kakera}';
                li.appendChild(dollarElement);

                li.appendChild(document.createTextNode(' KP '));

                var kpElement = document.createElement('strong');
                kpElement.textContent = '{kp}%';
                li.appendChild(kpElement);

                li.appendChild(document.createTextNode(' Marry: '));

                var marryElement = document.createElement('strong');
                marryElement.className = 'status-true';
                marryElement.textContent = '{marry}';
                li.appendChild(marryElement);

                li.appendChild(document.createTextNode(' RT: '));

                var rtElement = document.createElement('strong');
                rtElement.className = 'status-{rt}';
                rtElement.textContent = '{rt}';
                li.appendChild(rtElement);

                div.appendChild(li);
                ";
                IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)logBrowser;
                jsExecutor.ExecuteScript(script);
            } // Send the log to the web console

            void SendMessage(string message, ref NavigatorInstance nav, ref DiscordAcc acc, bool noDelay = false, double speedMultiplier = 1.00)
            {
                nav.driver.SwitchTo().Window(nav.windowHandler[acc.driverTab]);
                ((IJavaScriptExecutor)nav.driver).ExecuteScript("window.focus();");
                IWebElement textField = nav.driver.FindElement(By.CssSelector(SelPaths.TextBox.cssSelector));
                if (noDelay)
                {
                    try 
                    { 
                        nav.actions.Click(textField).SendKeys(message).SendKeys(Keys.Enter).Build().Perform();
                    }
                    catch { Console.WriteLine("(NoDelay) The following message failed: " + message); }
                }
                else
                {
                    foreach (char character in message)
                    {
                        if (character == ' ')
                        {
                            RandomSleep((0.1 * speedMultiplier), (0.2 * speedMultiplier));
                            nav.actions.SendKeys($"{character}").Build().Perform();
                        }
                        else
                        {
                            RandomSleep((0.05 * speedMultiplier), (0.1 * speedMultiplier));
                            nav.actions.SendKeys($"{character}").Build().Perform();
                        }
                    }
                    RandomSleep((0.1 * speedMultiplier), (0.2 * speedMultiplier));
                    nav.actions.SendKeys(Keys.Enter).Build().Perform();
                }
            } // Send a message on the current discord text field

            void WriteGroupDiagn(int group)
            {
                for (int j = 0; j < list[group].Length; j++)
                {
                    WriteToConsole($"{list[group][j].Nick} : [ KP: {list[group][j].kakeraPower} ] [ Marry: {list[group][j].marriable} ] [ RT: {list[group][j].rt} ] [ Kakeras: {list[group][j].kakeras} ]");
                }
            } // Display the status of all accounts

            void FullGroupDiag(ref NavigatorInstance nav)
            {
                int[] accIndex = AccountIndexFinder(NavActualAcc(ref nav));
                DiscordAcc acc = list[accIndex[0]][accIndex[1]];
                for (int i = 0; i < list[accIndex[0]].Length; i++)
                {
                    SwitchAccTabById(list[accIndex[0]][i].Id, ref nav);
                    SendDiagn(ref nav, acc: ref acc);
                    Console.Clear();
                    WriteGroupDiagn(accIndex[0]);
                }
            } // Realize the 'SendDiag' in all accounts in the current group

            void SendDiagn(ref NavigatorInstance nav, ref DiscordAcc acc, bool write = false, bool doOnDiagChannel = false)
            {
                try
                {
                    if (doOnDiagChannel) { SwitchChannel(nav: ref nav, channelTarget: $"diagnosticos-{acc.navHostIndex + 1}"); }
                    RandomSleep(0.3, 0.6);
                    SendMessage(message: "$tu", ref nav, ref acc, noDelay: true);
                    RandomSleep(1.0, 1.5);
                    ReadLastMsgId(ref nav, completeInfo: false);
                    IWebElement OwnLastMessageElement = LastMessageElement(ref nav);
                    string ownMessageId = RetrieveMessageId(OwnLastMessageElement);
                    string[] infoList = OwnLastMessageElement.FindElement(By.CssSelector($"#message-content-{ownMessageId}")).Text.Split("\n");
                    if (infoList[0].Contains("pode")) { acc.marriable = true; } else { acc.marriable = false; }
                    if (infoList[1].Contains("pronto")) { acc.rt = true; } else { acc.rt = false; }
                    string power = infoList[2].Split(" ")[1];
                    acc.kakeraPower = int.Parse(power[0..(power.Length - 2)]);
                    acc.rolls = int.Parse(infoList[3].Split(" ")[2]);
                    acc.kakeras = int.Parse(infoList[5].Split(" ")[1]);

                    SendDiagLog(acc.Nick, acc.marriable, acc.rt, acc.kakeraPower, acc.kakeras);
                    if (doOnDiagChannel) { SwitchChannel(ref nav, "rolls-2"); }
                }
                catch { WriteToConsole($"diagnostic on {acc.Nick} fail!"); SwitchChannel(ref nav, "rolls-2"); }
            } // Retrieve the status of the selected Account

            void CheckKu(ref NavigatorInstance nav, string accountId)
            {
                int[] index = AccountIndexFinder(accountId);
                DiscordAcc acc = nav.accounts[index[1]];
                SendMessage("$ku", ref nav, ref acc, false);
                RandomSleep(0.2, 0.4);
                IWebElement messageElement;
                if (!ExpectMessage(ref nav, "$ku", out messageElement))
                {
                    string messageId = messageElement.GetAttribute("id");
                    IReadOnlyCollection<IWebElement> strongList = messageElement.FindElement(By.CssSelector($"#message-content-{messageId}")).FindElements(By.TagName("strong"));
                    acc.kakeraPower = int.Parse(strongList.First().Text);
                    acc.kakeras = int.Parse(strongList.Last().Text);
                }
            }

            void CheckMu(ref NavigatorInstance nav, string accountId)
            {
                int[] index = AccountIndexFinder(accountId);
                DiscordAcc acc = nav.accounts[index[1]];
                SendMessage("$mu", ref nav, ref acc, false);
                RandomSleep(0.2, 0.4);
                IWebElement messageElement;
                if (!ExpectMessage(ref nav, "$mu", out messageElement))
                {
                    string messageId = messageElement.GetAttribute("id");
                    IWebElement textHolder = messageElement.FindElement(By.CssSelector($"#message-content-{messageId}"));
                    IReadOnlyCollection<IWebElement> strongList = textHolder.FindElements(By.TagName("strong"));
                    string text = textHolder.Text;

                    if (text.Contains("você pode se casar agora mesmo")) acc.marriable = true;
                    else acc.marriable = false;

                    int minutesToReset;
                    if (strongList.Last().Text.Contains("h"))
                    {
                        string[] time = strongList.Last().Text.Split("h");
                        minutesToReset = (int.Parse(time[0]) * 60) + int.Parse(time[1]);
                    }
                    else
                    {
                        minutesToReset = int.Parse(strongList.Last().Text);
                    }
                    if (minutesToReset < 3) lessThan3Min = true;
                    else lessThan3Min = false;
                }
            }

            void CheckRu(ref NavigatorInstance nav, string accountId)
            {
                int[] index = AccountIndexFinder(accountId);
                DiscordAcc acc = nav.accounts[index[1]];
                SendMessage("$ru", ref nav, ref acc, false);
                RandomSleep(0.2, 0.4);
                IWebElement messageElement;
                if (!ExpectMessage(ref nav, "$ru", out messageElement))
                {
                    string messageId = messageElement.GetAttribute("id");
                    IWebElement textHolder = messageElement.FindElement(By.CssSelector($"#message-content-{messageId}"));
                    IList<IWebElement> strongList = textHolder.FindElements(By.TagName("strong"));

                    if (strongList.Count() == 3) acc.rolls = int.Parse(strongList[0].Text) + int.Parse(strongList[1].Text);
                    else acc.rolls = int.Parse(strongList[0].Text);
                }
            }

            // ================================

            int[] RetrieveMKPAcc(bool marry = false, bool kp = false)
            {
                for (int i = 0; i < chatBot.Length; i++)
                {
                    for (int j = 0; j < chatBot[i].accounts.Length; j++)
                    {
                        if (chatBot[i].accounts[j].isActive)
                        {
                            if (marry) 
                            {
                                bool marriable = chatBot[i].accounts[j].marriable;
                                if (marriable) return new int[] { i, j }; 
                            }
                            if (kp) 
                            {
                                if (chatBot[i].accounts[j].kakeraPower > kpUsage) 
                                { 
                                    return new int[] { i, j }; 
                                } 
                            }
                        }
                    }
                }
                WriteToConsole("No accounts with marry/kp");
                return new int[] { -1, -1 };
            } // Return the int[] { 0 Nav, 1 Account } of the first marriable Account

            void ReactMarry(string targetId)
            {
                int[] index = RetrieveMKPAcc(marry: true);
                if (index[0] == -1) return;
                int bot = index[0];
                int acc = index[1];
                SwitchAccTabById(targetId: chatBot[bot].accounts[acc].Id, nav: ref chatBot[bot]);
                TryConvertMessageToChar(ref chatBot[bot], targetId, ref charMarryTarget);
                if (ClickButton(ref chatBot[bot], targetId, 0))
                {
                    if (ConfirmMarry(targetId, ref chatBot[bot]))
                    {
                        chatBot[bot].accounts[acc].marriable = false;
                        SendMarryLog(charname: charMarryTarget.name, ownerName: chatBot[bot].accounts[acc].Nick, "true");
                    }
                    else
                    {
                        chatBot[bot].accounts[acc].marriable = false;
                        SendMarryLog(charname: charMarryTarget.name, ownerName: chatBot[bot].accounts[acc].Nick, "false");
                        ReactMarry(targetId);
                    }
                }

            } // Realizes a marry, based on messageId

            void ReactKakera(string targetId, string type, int buttonIndex)
            {
                int[] index = RetrieveMKPAcc(kp: true);
                if (index[0] == -1) return;
                int bot = index[0];
                int acc = index[1];
                SwitchAccTabById(targetId: chatBot[bot].accounts[acc].Id, nav: ref chatBot[bot]);

                TryConvertMessageToChar(ref chatBot[bot], targetId, ref charMarryTarget);
                if (ClickButton(ref chatBot[bot], targetId, buttonIndex))
                {
                    if (ConfirmKakeraReaction(targetId, ref chatBot[bot]))
                    {
                        int previousKp = chatBot[bot].accounts[acc].kakeraPower;
                        int nextKp = previousKp - kpUsage;
                        if (type != "P") { chatBot[bot].accounts[acc].kakeraPower = nextKp; }
                        SendKakeraLog(charName: charMarryTarget.name, ownerName: chatBot[bot].accounts[acc].Nick, result: true, type: type, amountEarned: kakeraEarnedOnReaction);
                    }
                    else
                    {
                        SendDiagn(ref chatBot[bot], ref chatBot[bot].accounts[acc]);
                        SendKakeraLog(charName: charMarryTarget.name, ownerName: chatBot[bot].accounts[acc].Nick, result: false, type: type);
                        ReactKakera(targetId, type, buttonIndex);
                    }
                }

            } // Reacts to a kakera button, based on messageId

            bool ClickButton(ref NavigatorInstance nav, string targetId, int buttonIndex)
            {
                RandomSleep(0.1, 0.4);
                IWebElement messageElement;
                if (TryRetrieveMessageElementById(ref nav, targetId, out messageElement))
                {
                    nav.actions.ScrollToElement(messageElement);
                    IWebElement buttonListHolder = messageElement.FindElement(By.XPath($"//*[@id=\"message-accessories-{targetId}\"]/div"));
                    buttonListHolder = buttonListHolder.FindElement(By.ClassName("children_f15443"));
                    IList<IWebElement> Buttons = buttonListHolder.FindElements(By.TagName("button"));
                    IWebElement buttonElement = Buttons[buttonIndex];
                    buttonElement.Click();
                    return true;
                }
                return false;

            } // Click on the first button, based on messageElement

            bool ConfirmMarry(string TargetId, ref NavigatorInstance nav)
            {

                for (int i = 0; i < 3; i++)
                {
                    RandomSleep(2.0, 2.2);
                    TryConvertMessageToChar(ref nav, TargetId, ref charMarryTarget);
                    if (charMarryTarget.owned) return true;
                }
                return false;
            } // Return the success or failure of ReactMarry

            bool ConfirmKakeraReaction(string TargetId, ref NavigatorInstance nav)
            {
                string negativeText = "Você não pode reagir a um kakera antes";
                for (int i = 0; i < 3; i++)
                {
                    RandomSleep(2.0, 2.2);
                    IWebElement messageElement = LastMessageElement(ref nav);
                    if (RetrieveMessageId(messageElement) == TargetId) return true;

                    messageText = messageElement.FindElement(By.CssSelector($"#message-content-{RetrieveMessageId(messageElement)}")).Text;

                    if (!messageText.Contains(negativeText)) { kakeraEarnedOnReaction = messageText.Split(" ")[^2]; return true; }
                }
                return false;
            } // Return the success or failure of ReactKakera

            // ================================

            void UserCommand(string command, ref NavigatorInstance nav)
            {
                command = command.Substring(3);
                string[] commandAttrList = command.Split(" ");
                switch (commandAttrList[0])
                {
                    // =================================================================================
                    case "fullDiag":
                        FullGroupDiag(ref nav);
                        break;

                    // =================================================================================

                    default:
                        // Code to handle any other commands
                        WriteToConsole("Unknown command!");
                        break;
                }
            }

            void EventTrigger(string targetId, ref NavigatorInstance[] instance)
            {
                if ((!lastChar.owned && lastChar.value > minimunKakera) && lastChar.messageId == targetId)
                {
                    ReactMarry(targetId);
                }

                if (lastChar.owned && lastChar.messageId == targetId) 
                {
                    int i = 0;
                    if (lastChar.buttonList is not null)
                    {
                        foreach (string buttonOnList in lastChar.buttonList)
                        {
                            if (allowedKakeraButtons.Any(button => button == buttonOnList))
                            {
                                ReactKakera(targetId, lastChar.buttonList[i], i);
                            }
                            i++;
                        }
                    }
                }

            }

            void WriteToConsole(string message)
            {
                if (writeToWebBrowser)
                {
                    string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"C:\Users\ale_5\Desktop\Projects\CSM_WebPage\log.json");
                    var logObject = new { Message = message, Timestamp = DateTime.Now };
                    string jsonString = JsonConvert.SerializeObject(logObject);
                    File.WriteAllText(filePath, jsonString);
                }
                else
                {
                    Console.WriteLine(message);
                }
            }

            // ================================

            string EmbbedDescText(IWebElement message)
            {
                return message.FindElement(By.CssSelector($"{SelPaths.EmbbedDesc.cssWithIdStart}{last_message_id}{SelPaths.EmbbedDesc.cssWithIdEnd}")).Text;
            }

            // ================================

            void CheckHarem(string targetId)
            {
                int[] index = AccountIndexFinder(targetId);
                if (index[0] == -1) return;
                int bot = index[0];
                int acc = index[1];
                ref DiscordAcc account = ref chatBot[bot].accounts[acc];
                ref NavigatorInstance nav = ref chatBot[bot];
                SendMessage("$mm", ref nav, ref account, noDelay: true);
                RandomSleep(3.5, 4.5);
                string charListEmbedId = last_message_id;
                Console.WriteLine(EmbbedDescText(LastMessageElement(ref nav)));
            }

            void GiveMeEveryWaifu(ref NavigatorInstance[] navs)
            {
                {
                    for (int i = 0; i < navs.Length; i++)
                    {
                        for (int j = 0; j < navs[i].accounts.Length; j++)
                        {
                            if (navs[i].accounts[j].isActive)
                            {
                                CheckHarem(navs[i].accounts[j].Id);
                            }
                        }
                    }
                }
            }

            void PayTheTaxes(ref NavigatorInstance[] navs)
            {
                for (int i = 0; i < navs.Length; i++)
                {
                    for (int j = 0; j < navs[i].accounts.Length; j++)
                    {
                        if (navs[i].accounts[j].isActive && navs[i].accounts[j].kakeras > 0)
                        {
                            string kakeraMessage = $"$givek <@{masterAcc[0].accounts[0].Id}> {navs[i].accounts[j].kakeras}";
                            SendMessage(kakeraMessage, ref navs[i], ref navs[i].accounts[j], noDelay: true);
                            RandomSleep(2.3, 2.7);
                            SendMessage("y", ref navs[i], ref navs[i].accounts[j]);
                            navs[i].accounts[j].kakeras = 0;
                        }
                    }
                }
            }

            #endregion

            // =================================================================================

            #region Main Program ===============================================================

            DiscordAcc[] discordAccounts = list[groupacc];
            string userInput;

            do
            {
                if (configMode)
                {
                    ReadLastMsgId(ref chatBot[0], completeInfo: true);
                    EventTrigger(last_message_id, ref chatBot);
                    if (ClickBottomChat(ref chatBot[0])) RandomSleep(3.5, 4.5);

                    Console.WriteLine($"Entry a Command, my master...\n >>> ");
                    userInput = Console.ReadLine()!;
                    if (userInput == "X") break;
                    if (userInput == "A") StartRolling(ref masterAcc[0]);
                    if (userInput == "U") SendUs(ref masterAcc[0]);
                    if (userInput == "L") RollLoop(ref masterAcc[0]);
                    if (userInput == "P") PayTheTaxes(ref chatBot);
                }
                else
                {
                    // Console.Clear();
                    ReadLastMsgId(ref chatBot[0], completeInfo: true);
                    userInput = Console.ReadLine()!;
                    if (userInput == "X") break;
                }
            }
            while (true);
            // ================================
            if (singprof) chatBot[0].driver.Quit();
            else { for (int i = 0; i < instancesNumber; i++) { chatBot[i].driver.Quit(); } }
            #endregion

            // =================================================================================

            #region Classes ====================================================================
        }

    }

    // =================================================================================
    // Classes

    class NavigatorInstance
    {
        public int ownIndex;
        public ReadOnlyCollection<string> windowHandler;
        public IWebDriver driver;
        public bool reader = false;
        public Actions actions;
        public string diagChannel;
        public DiscordAcc[] accounts;
    }

    class DiscordAcc
    {
        public bool isActive = false;
        public int navHostIndex;
        public NavigatorInstance? navHost;
        public int driverTab;
        public string? Id;
        public string? Nick;
        public string? Discriminator;
        public bool marriable = true;
        public bool rt = true;
        public int rolls;
        public int kakeraPower = 0;
        public int kakeras = 0;
    }

    class Character
    {
        public string? messageId;
        public string? name;
        public string? serie;
        public int? value;
        public bool owned = true;
        public bool hasButton = false;
        public string[]? buttonList;
    }
}

#endregion

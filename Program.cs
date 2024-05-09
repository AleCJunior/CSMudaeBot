using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System.Collections.ObjectModel;

namespace MudaeBOT
{
    class Program
    {
        static StreamWriter logStreamWriter;

        static void Main()
        {
            // =================================================================================

            #region Standard Vars ==============================================================
            logStreamWriter = new StreamWriter("log.txt");
            logStreamWriter.AutoFlush = true;
            // ================================
            string discordUrl = InternKeys.discordUrl;
            string serverid = InternKeys.serverid;
            string channelid = InternKeys.mainChannelid;
            // ================================
            string[] diagChannel = AccKeys.diagChannel;
            // ================================
            DiscordAcc[][] list = AccKeys.list;
            // ================================
            bool turnFarmUS = false;
            bool doLogging = true;
            bool KakeraCriteria = true;
            // ================================
            int minimunKakera = 300;
            // ================================
            IWebElement div_scroller;
            IReadOnlyList<IWebElement> message_list;
            IReadOnlyList<IWebElement> channel_list;
            IReadOnlyCollection<IWebElement> MessageContentHolderElementList;
            IWebElement? MessageContentHolderElement;
            IWebElement last_message;
            // ================================
            Character lastChar = new Character { name = null, buttonType = null, messageId = null};
            string[] MessageContentHolder;
            string last_message_id;
            string Character;
            string Serie;
            string Kakera;
            string ButtomTypeText;
            string EmbFooterInfo;
            string fullEmbDesc;
            // ================================
            bool lastMsgSucces;
            bool charMarriedStatus;
            bool new_message = true;
            bool isEmbbed;
            bool isCharacter;
            // ================================
            string? messageIdHolder = "0";
            string messageOwnerName;
            string messageTimeStamp;
            string messageText;
            // ================================
            string[] allowedKakeraButtons = new string[] { "P", "O", "R", "W", "L" };
            // ================================
            #endregion

            // =================================================================================

            #region Functions ==================================================================

            #region LoadElement ( string path, int limitTry, int interval, string SelectorMethod, bool byDrive, bool runAll, IWebDriver driver, SelPath? PathList, IWebElement Element )
            IWebElement LoadElement(
            NavigatorInstance nav,
            string path = "body",
            int limitTry = 3,
            int interval = 1,
            string SelectorMethod = "css",
            bool byDrive = false,
            bool runAll = false,
            SelPath? PathList = null,
            IWebElement? Element = null
            ) {
                ClickBottomChat(nav); RandomSleep(1.0, 1.2);
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
                        Thread.Sleep(interval * 1000);
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

            void ClickBottomChat(NavigatorInstance nav)
            {
                bool foundBottomChatButton;
                try
                {
                    IWebElement chatdownButton = nav.driver.FindElement(By.CssSelector(SelPaths.BottonScrollerButton.cssSelector));
                    chatdownButton.Click();
                    foundBottomChatButton = true;
                }
                catch
                {
                    foundBottomChatButton = false;
                }
            } // Goes to the bottom of the chat

            string NavActualAcc(NavigatorInstance nav)
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
            }

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
            }

            // ================================

            IWebElement LastMessageElement(NavigatorInstance nav)
            {
                ClickBottomChat(nav); RandomSleep(1.0, 1.2);
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
            }

            bool TryRetrieveMessageElementById(NavigatorInstance nav, string messageTargetId, out IWebElement messageElement)
            {
                ClickBottomChat(nav); RandomSleep(1.0, 1.2);
                try
                {
                    IWebElement divScroller = nav.driver.FindElement(By.CssSelector(SelPaths.scrollerBox.cssSelector));
                    IReadOnlyList<IWebElement> messageList = divScroller.FindElements(By.TagName("li"));
                    foreach (IWebElement message in messageList)
                    {
                        if (messageTargetId == RetrieveMessageId(last_message)) { messageElement = message; return true; }
                    }
                    messageElement = null; return false;
                }
                catch
                {
                    messageElement = null; return false;
                }
            }

            string RetrieveMessageId(IWebElement message)
            {
                string message_id = message.GetAttribute("id");
                return message_id.Split('-')[^1];
            }

            void ReadLastMsgId(NavigatorInstance nav, bool completeInfo = false)
            {
                ClickBottomChat(nav); RandomSleep(1.0, 1.2);
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

                if (completeInfo && (lastMsgSucces && new_message)) ReadLastMessage(nav);
            }

            void ReadLastMessage(NavigatorInstance nav)
            {
                ClickBottomChat(nav);
                // =================================================================================
                // Identify new messages

                try // Get span list
                {
                    MessageContentHolderElement = last_message.FindElement(By.CssSelector($"#chat-messages-{channelid}-{last_message_id} > div > div.contents_d3ae0d"));
                    MessageContentHolderElementList = MessageContentHolderElement.FindElements(By.TagName("span"));
                    MessageContentHolder = new string[MessageContentHolderElementList.Count];
                    int i = 0;
                    foreach (IWebElement element in MessageContentHolderElementList)
                    {
                        MessageContentHolder[i] = element.Text;
                        i++;
                    }
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
                    if (messageText[0..2].Contains("c$")) { UserCommand(messageText, nav); }

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
                try
                {
                    IReadOnlyCollection<IWebElement> ButtomList = last_message.FindElements(By.TagName("button"));
                    IWebElement buttonElement = ButtomList.Last();
                    buttonElement = buttonElement.FindElement(By.TagName("img"));
                    ButtomTypeText = buttonElement.GetAttribute("alt");
                    if (isCharacter) lastChar.hasButton = true;
                }
                catch { ButtomTypeText = "null"; if (isCharacter) lastChar.hasButton = false; }
                if (isCharacter) lastChar.buttonType = ButtomTypeText;

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
                    int spanIndex = 0;
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
                            WriteToConsole($"ButtomTypeText: {ButtomTypeText}");
                            WriteToConsole($"Embbed Footer: {EmbFooterInfo}");
                            WriteToConsole($"Marryed: {charMarriedStatus}");
                        }

                    }
                    else
                    {
                        WriteToConsole($"MSG Text: {messageText}");
                    }
                }

            }

            // ================================

            void SwitchChannel(NavigatorInstance nav, string channelTarget)
            {
                try
                {
                    for (int i = 0; i < 3; i++)
                    {
                        RandomSleep(0.5, 1.1);
                        IWebElement channelPanel = LoadElement(nav, byDrive: true, runAll:true, PathList: SelPaths.ChannelList);
                        IReadOnlyCollection<IWebElement> channelList = channelPanel.FindElements(By.TagName("li")); WriteToConsole("Step 2");
                        foreach (IWebElement channel in channelList)
                        {
                            string channelName = channel.GetAttribute("data-dnd-name");
                            if (channelName == channelTarget) { channel.Click(); LoadElement(nav); break; }
                        }
                        break;
                    }
                }
                catch { }

            } 

            void SwitchAcc(NavigatorInstance nav, string targetId)
            {
                try
                {
                    string actualNavAcc = NavActualAcc(nav);
                    if (actualNavAcc != targetId)
                    {
                        IWebElement accountButton1 = nav.driver.FindElement(By.CssSelector(SelPaths.switchAcc1.cssSelector));
                        accountButton1.Click();
                        IWebElement accountButton2 = LoadElement(nav: nav, path: "#account-switch-account > div.label__563c3", SelectorMethod: "css", byDrive: true);
                        // IWebElement accountButton2 = driver.FindElement(By.CssSelector("#account-switch-account > div.label__563c3"));
                        accountButton2.Click();
                        IWebElement accountButton3 = LoadElement(nav: nav, path: SelPaths.switchAcc2.cssSelector, SelectorMethod: "css", byDrive: true);
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
                        nav.driver.Navigate().GoToUrl($"{discordUrl}/{serverid}/{channelid}");
                        ClickBottomChat(nav);
                    }
                }
                catch (Exception ex) { }
            }

            void SwitchAccTab(string targetId, NavigatorInstance nav)
            {
                int[] accIndex = AccountIndexFinder(targetId);
                nav.driver.SwitchTo().Window(nav.windowHandler[accIndex[1]]);
                ((IJavaScriptExecutor)nav.driver).ExecuteScript("window.focus();");
                ((IJavaScriptExecutor)nav.driver).ExecuteScript("arguments[0].scrollIntoView(true);", LastMessageElement(nav));
                ClickBottomChat(nav); RandomSleep(0.5, 1.0);
            }

            // ================================

            void SendMessage(string message, NavigatorInstance nav, bool noDelay = false, double speedMultiplier = 1.00)
            {
                IWebElement textField = LoadElement(path:SelPaths.TextBox.cssSelector, byDrive: true, nav: nav);
                if (noDelay)
                {
                    nav.actions.Click(textField).SendKeys(message).SendKeys(Keys.Enter).Build().Perform();
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
            }

            void WriteGroupDiagn(int group)
            {
                for (int j = 0; j < list[group].Length; j++)
                {
                    WriteToConsole($"{list[group][j].Nick} : [ KP: {list[group][j].kakeraPower} ] [ Marry: {list[group][j].marriable} ] ", inSameLine: true);
                    WriteToConsole($"[ RT: {list[group][j].rt} ] [ Kakeras: {list[group][j].kakeras} ]");
                }
            }

            void FullGroupDiag(NavigatorInstance nav)
            {
                int[] accIndex = AccountIndexFinder(NavActualAcc(nav));
                DiscordAcc acc = list[accIndex[0]][accIndex[1]];
                for (int i = 0; i < list[accIndex[0]].Length; i++)
                {
                    SwitchAccTab(list[accIndex[0]][i].Id, nav);
                    SendDiagn(nav, acc: acc);
                    Console.Clear();
                    WriteGroupDiagn(accIndex[0]);
                }
            }

            void SendDiagn(NavigatorInstance nav, bool write = false, DiscordAcc acc = null)
            {
                if (acc is null)
                {
                    int[] accIndex = AccountIndexFinder(NavActualAcc(nav));
                    acc = list[accIndex[0]][accIndex[1]];
                }
                try
                {
                    SwitchChannel(nav: nav, channelTarget: $"diagnosticos-{acc.navHostIndex + 1}");
                    SendMessage(message: "$tu", nav, noDelay: true);
                    RandomSleep(2.0, 2.5);
                    ReadLastMsgId(nav, completeInfo: false);
                    IWebElement OwnLastMessageElement = LastMessageElement(nav);
                    string ownMessageId = RetrieveMessageId(OwnLastMessageElement);
                    string[] infoList = OwnLastMessageElement.FindElement(By.CssSelector($"#message-content-{ownMessageId}")).Text.Split("\n");
                    if (infoList[0].Contains("pode")) { acc.marriable = true; } else { acc.marriable = false; }
                    if (infoList[1].Contains("pronto")) { acc.rt = true; } else { acc.rt = false; }
                    string power = infoList[2].Split(" ")[1];
                    acc.kakeraPower = int.Parse(power[0..(power.Length - 2)]);
                    acc.rolls = int.Parse(infoList[3].Split(" ")[2]);
                    acc.kakeras = int.Parse(infoList[5].Split(" ")[1]);

                    if (write)
                    {
                        WriteToConsole($"{acc.Nick} : [ KP: {acc.kakeraPower} ] [ Marry: {acc.marriable} ] ");
                        WriteToConsole($"[ RT: {acc.rt} ] [ Kakeras: {acc.kakeras} ]");
                    }
                    SwitchChannel(nav, "rolls-2");
                }
                catch { WriteToConsole($"diagnostic on {acc.Nick} fail!"); SwitchChannel(nav, "rolls-2"); }
            }

            // ================================

            int[] RetrieveMarriableAcc()
            {
                for (int i = 0; i < list.Length; i++)
                {
                    for (int j = 0; j < list[i].Length; j++)
                    {
                        if (list[i][j].isActive && list[i][j].marriable) return new int[] { i, j };
                    }
                }
                return new int[] { 0, 0 };
            }

            int[] RetrieveKakeraPowerAcc(string buttonType)
            {
                for (int i = 0; i < list.Length; i++)
                {
                    for (int j = 0; j < list[i].Length; j++)
                    {
                        if (list[i][j].isActive && list[i][j].marriable) return new int[] { i, j };
                    }
                }
                return new int[] { 0, 0 };
            }

            void AnalyzeChar(Character character, NavigatorInstance[] instance )
            {
                if (character == null) return;
                if (!character.owned && character.value >= minimunKakera)
                {
                    // ReactMarry();
                }
                else
                { if (character.hasButton)
                    {
                        foreach (string targetButton in allowedKakeraButtons)
                        {
                            if (targetButton == $"{character.buttonType[^1]}")
                            {
                                // ReactKakera();
                                
                            }
                        }
                    }
                }
            }

            void ReactMarry(string targetId, NavigatorInstance[] instance)
            {
                int[] index = RetrieveMarriableAcc();
                int bot = index[0];
                int acc = index[1];
                IWebElement messageElement;
                NavigatorInstance nav = instance[bot];
                SwitchAccTab(targetId: list[bot][acc].Id, nav: nav);

                if (!TryRetrieveMessageElementById(nav, targetId, out messageElement))
                {
                    ClickButton(messageElement);
                }
            }

            void ReactKakera(string targetId, NavigatorInstance[] instance)
            {
                int[] index = RetrieveMarriableAcc();
                int bot = index[0];
                int acc = index[1];
                IWebElement messageElement;
                NavigatorInstance nav = instance[bot];
                SwitchAccTab(targetId: list[bot][acc].Id, nav: nav);

                if (!TryRetrieveMessageElementById(nav, targetId, out messageElement))
                {
                    ClickButton(messageElement);
                }
            }

            void ClickButton(IWebElement messageElement)
            {
                IReadOnlyCollection<IWebElement> Buttons = messageElement.FindElements(By.TagName("button"));
                IWebElement buttonElement = Buttons.Last();
                buttonElement.Click();
            }

            // ================================

            void UserCommand(string command, NavigatorInstance nav)
            {
                command = command.Substring(3);
                string[] commandAttrList = command.Split(" ");
                switch (commandAttrList[0])
                {
                    // =================================================================================

                    case "sengDiag":
                        SendDiagn(nav);
                        break;

                    // =================================================================================

                    case "fullDiag":
                        FullGroupDiag(nav);
                        break;

                    // =================================================================================

                    default:
                        // Code to handle any other commands
                        WriteToConsole("Unknown command!");
                        break;
                }
            }

            void EventTrigger()
            {

            }
            #endregion

            // =================================================================================

            #region Control Vars - Setup inicialization ========================================

            bool finalMode = false;
            bool singprof = true;
            bool singleAcc = true;
            bool configMode = false;
            bool asyncTasks = false;
            bool master = true;

            int accounts = 5;
            int instancesNumber = 3;
            int cprofile = 0;

            int groupacc = cprofile;
            if (singprof) { instancesNumber = 1; }
            if (configMode) { singprof = true; }
            if (singleAcc) { singprof = true; accounts = 1; }
            if (finalMode) { singprof = false; singleAcc = false; configMode = false; asyncTasks = false; }

            string chromeDriverPath = @"C:\Users\ale_5\Desktop\Projects\CS-MudaeBot\chromedriver.exe";
            string profileDirectory = @"C:\Users\ale_5\Desktop\Projects\CS-MudaeBot\chromeprofiles\";

            NavigatorInstance[] chatBot;
            chatBot = new NavigatorInstance[instancesNumber];

            // ================================

            NavigatorInitializer(chromeProfile: cprofile, instances: instancesNumber, singProf: singprof, accountNumber: accounts, asyncron: asyncTasks).Wait();

            if (finalMode || master)
            {
                ChromeOptions options = new ChromeOptions();
                options.AddArgument($@"--user-data-dir={profileDirectory}\master");
                NavigatorInstance masterAcc = new NavigatorInstance { driver = new ChromeDriver(chromeDriverPath, options) };
                masterAcc.actions = new Actions(masterAcc.driver);
                masterAcc.windowHandler = masterAcc.driver.WindowHandles;
                masterAcc.driver.Navigate().GoToUrl($"{discordUrl}/{serverid}/{channelid}");
                ClickBottomChat(masterAcc);
            }

            #endregion

            // =================================================================================

            #region Initialization Functions ===================================================

            async Task NavigatorInitializer(int chromeProfile, int instances, int accountNumber, bool singProf = false, bool asyncron = false)
            {
                if (asyncron)
                {
                    Task[] tasks = new Task[instances];

                    WriteToConsole($"Working with {instances} async instances");
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
                        WriteToConsole($"Working with {instances} async instances");
                        int navIndex = i;
                        WriteToConsole($"starting {i} instance");
                        await awaitedNavInit(singProf, chromeProfile, navIndex, accountNumber);
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
                DiscordAcc[] accounts = list[navIndexer];
                for (int j = 1; j < accounts.Length; j++) { ((IJavaScriptExecutor)chatBot[navIndexer].driver).ExecuteScript("window.open();"); }
                chatBot[navIndexer].windowHandler = chatBot[navIndexer].driver.WindowHandles;

                for (int k = 0; k < accountNumber; k++)
                {
                    int accIndexer = k;
                    WriteToConsole($"instance {navIndexer}: ", inSameLine:true);
                    AccountInitializer(accounts, accIndexer, navIndexer, chatBot[navIndexer]); 
                }

            }

            void AccountInitializer(DiscordAcc[] accounts, int accIndexer, int navIndexer, NavigatorInstance nav)
            {
                accounts[navIndexer].navHostIndex = navIndexer;
                accounts[navIndexer].navHost = chatBot[navIndexer];
                WriteToConsole($"Initializing {accounts[accIndexer].Nick}");
                RandomSleep(1.0, 2.5);
                nav.driver.SwitchTo().Window(nav.windowHandler[accIndexer]);
                ((IJavaScriptExecutor)nav.driver).ExecuteScript("window.focus();");
                nav.driver.Navigate().GoToUrl($"{discordUrl}/{serverid}/{channelid}");
                LoadElement(nav);
                accounts[accIndexer].driverTab = accIndexer;
                RandomSleep(1.0, 2.5);
                SwitchAcc(nav, accounts[accIndexer].Id);
                RandomSleep(1.0, 2.5);
                SendDiagn(nav:nav, acc:accounts[accIndexer]);
            }

            #endregion

            // =================================================================================

            #region Testing Functions ==========================================================

            void DinamicCommand()
            {
                int navIndex;
                int accIndex;
                WriteToConsole($"Select the Nav Instance [ 0 ~ {chatBot.Length - 1} ]: ");
                while ((int.TryParse(Console.ReadLine()!, out navIndex) && navIndex < chatBot.Length)) { WriteToConsole("Invalid Option!"); };
                WriteToConsole($"Select the Instance Acc [ 0 ~ {chatBot.Length - 1} ]: ");
                while ((int.TryParse(Console.ReadLine()!, out accIndex) && accIndex < chatBot.Length)) { WriteToConsole("Invalid Option!"); };
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
                    ReadLastMsgId(chatBot[0], completeInfo: true);
                    ClickBottomChat(chatBot[0]);
                    userInput = Console.ReadLine()!;
                    if (userInput == "X") break;
                }
                else
                {
                    // Console.Clear();
                    ReadLastMsgId(chatBot[0], completeInfo: true);
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
        }

        static void WriteToConsole(string message, bool inSameLine = false)
        {
            {
                if (inSameLine) { Console.Write(message); }
                else Console.WriteLine(message); // Write to the console
                logStreamWriter.WriteLine(message);
            }
        }
    }

    // =================================================================================
    // Classes

    class NavigatorInstance
    {
        public ReadOnlyCollection<string> windowHandler;
        public IWebDriver driver;
        public bool reader = false;
        public Actions actions;
        public string diagChannel;
    }

    class DiscordAcc
    {
        public bool isActive = false;
        public int navHostIndex;
        public NavigatorInstance navHost;
        public int driverTab;
        public string Id;
        public string Nick;
        public string? Discriminator;
        public bool marriable = false;
        public bool rt = false;
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
        public string? buttonType;
    }
}
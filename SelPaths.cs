namespace MudaeBOT
{
    public static class SelPaths
    {
        public static SelPath lBGuild = new SelPath
        {
            cssSelector = "#app-mount > div.appAsidePanelWrapper__5e6e2 > div.notAppAsidePanel__95814 > div.app_b1f720 > div > div.layers__1c917.layers_a23c37 > div > div > div > div > div.sidebar_e031be",
            xpath = "//*[@id=\"app-mount\"]/div[2]/div[1]/div[1]/div/div[2]/div/div/div/div/div[1]",
            fullxpath = "/html/body/div[2]/div[2]/div[1]/div[1]/div/div[2]/div/div/div/div/div[1]"
        };

        public static SelPath lBChannels = new SelPath
        {
            cssSelector = "#app-mount > div.appAsidePanelWrapper__5e6e2 > div.notAppAsidePanel__95814 > div.app_b1f720 > div > div.layers__1c917.layers_a23c37 > div > div > div > div > div.sidebar_e031be",
            xpath = "//*[@id=\"app-mount\"]/div[2]/div[1]/div[1]/div/div[2]/div/div/div/div/div[1]",
            fullxpath = "/html/body/div[2]/div[2]/div[1]/div[1]/div/div[2]/div/div/div/div/div[1]"
        };

        public static SelPath scrollerBox = new SelPath
        {
            cssSelector = "#app-mount > div.appAsidePanelWrapper__5e6e2 > div.notAppAsidePanel__95814 > div.app_b1f720 > div > div.layers__1c917.layers_a23c37 > div > div > div > div > div.chat__52833 > div.content__01e65 > main > div.messagesWrapper_add28b.group-spacing-0 > div > div > ol",
            xpath = "//*[@id=\"app-mount\"]/div[2]/div[1]/div[1]/div/div[2]/div/div/div/div/div[1]/div[3]/main/div[1]/div/div/ol",
            fullxpath = "/html/body/div[2]/div[2]/div[1]/div[1]/div/div[2]/div/div/div/div/div[1]/div[3]/main/div[1]/div/div/ol"
        };

        public static SelPath ChannelList = new SelPath
        {
            cssSelector = "#channels > ul",
            xpath = "//*[@id=\"channels\"]/ul",
            fullxpath = "/html/body/div[1]/div[2]/div[1]/div[1]/div/div[2]/div/div/div/div/div[1]/nav/div[4]/ul",
            tagClass = "content__690c5",
            tagName = "ul"
        };

        public static SelPath userIdent = new SelPath
        {
            cssSelector = "#app-mount > div.appAsidePanelWrapper-ev4hlp > div.notAppAsidePanel-3yzkgB > div.app-3xd6d0 > div > div.layers-OrUESM.layers-1YQhyW > div > div > div > div > div.sidebar-1tnWFu > section > div.container-YkUktl > div.avatarWrapper-1B9FTW.withTagAsButton-OsgQ9v > div.nameTag-sc-gpq.canCopy-IgTwyV > div.defaultColor-1EVLSt.text-sm-normal-AEQz4v.usernameContainer-3PPkWq > div",
            xpath = "",
            fullxpath = ""
        };

        public static SelPath BottonScrollerButton = new SelPath
        {
            cssSelector = "#app-mount > div.appAsidePanelWrapper__5e6e2 > div.notAppAsidePanel__95814 > div.app_b1f720 > div > div.layers__1c917.layers_a23c37 > div > div > div > div > div.chat__52833 > div.content__01e65 > main > div.messagesWrapper_add28b.group-spacing-0 > div.jumpToPresentBar__69174.barBase__8839d > button.barButtonMain__984b4.barButtonBase__5e4cf",
            xpath = "//*[@id=\"app-mount\"]/div[2]/div[1]/div[1]/div/div[2]/div/div/div/div/div[2]/div[3]/main/div[1]/div[2]/button[1]",
            fullxpath = "/html/body/div[1]/div[2]/div[1]/div[1]/div/div[2]/div/div/div/div/div[2]/div[3]/main/div[1]/div[2]/button[1]"
        };

        public static SelPath EmbbedAuthor = new SelPath
        {
            cssWithIdStart = "#message-accessories-",
            cssWithIdEnd = " > article > div > div > div.embedAuthor__3e899.embedMargin__99b82 > span"
        };

        public static SelPath EmbbedDesc = new SelPath
        {
            cssWithIdStart = "#message-accessories-",
            cssWithIdEnd = " > article > div > div > div.embedDescription_f5043f.embedMargin__99b82"
        };

        public static SelPath EmbbedFooter = new SelPath
        {
            cssWithIdStart = "#message-accessories-",
            cssWithIdEnd = " > article > div > div > div.embedFooter_c26cec.embedMargin__99b82"
        };

        public static SelPath EmbbedFooterSpan = new SelPath
        {
            cssWithIdStart = "#message-accessories-",
            cssWithIdEnd = " > article > div > div > div.embedFooter_c26cec.embedMargin__99b82 > span"
        };

        public static SelPath ReactButton = new SelPath
        {
            cssWithIdStart = "#message-accessories-",
            cssWithIdEnd = " > div > div > div > button"
        };

        public static SelPath ReactButtomInfo = new SelPath
        {
            cssWithIdStart = "#message-accessories-",
            cssWithIdEnd = " > div > div > div > button > div > div"
        };
        public static SelPath TextBox = new SelPath
        {
            fullxpath = "/html/body/div[1]/div[2]/div[1]/div[1]/div/div[2]/div/div/div/div/div[2]/div[3]/main/form/div/div[1]/div/div[3]/div/div[2]",
            xpath = "//*[@id=\"app-mount\"]/div[2]/div[1]/div[1]/div/div[2]/div/div/div/div/div[2]/div[3]/main/form/div/div[1]/div/div[3]/div/div[2]",
            cssSelector = "#app-mount > div.appAsidePanelWrapper__5e6e2 > div.notAppAsidePanel__95814 > div.app_b1f720 > div > div.layers__1c917.layers_a23c37 > div > div > div > div > div.chat__52833 > div.content__01e65 > main > form > div > div.scrollableContainer_ff917f.themedBackground__3a4c0 > div > div.textArea_a86690.textAreaSlate__8578d.slateContainer__1d1fd > div > div"
        };

        public static SelPath msgOwnerPic = new SelPath
        {
            tagClass = "username__0b0e7 desaturateUserColors_eb6bd2 clickable__09456"
        };

        public static SelPath switchAcc1 = new SelPath
        {
            cssSelector = "#app-mount > div.appAsidePanelWrapper__5e6e2 > div.notAppAsidePanel__95814 > div.app_b1f720 > div > div.layers__1c917.layers_a23c37 > div > div > div > div > div.sidebar_e031be > section > div.container_debb33 > div.avatarWrapper__500a6.withTagAsButton_e22174"
        };

        public static SelPath switchAcc2 = new SelPath
        {
            cssSelector = "#app-mount > div.appAsidePanelWrapper__5e6e2 > div.notAppAsidePanel__95814 > div:nth-child(4) > div.layer_c14d31 > div > div > div > div > div > div.content_b28aab.thin__62e51.scrollerBase__65223 > div.list__4e6aa"
        };

        public static SelPath switchAcc3 = new SelPath
        {
            cssSelector = "#app-mount > div.appAsidePanelWrapper__5e6e2 > div.notAppAsidePanel__95814 > div:nth-child(4) > div.layer_c14d31 > div > div > div > div > div > div.content_b28aab.thin__62e51.scrollerBase__65223 > div.list__4e6aa > div:nth-child(1) > div > div.userActions__6955d > button.button__581d0.lookFilled__950dd.colorPrimary_ebe632.sizeMedium__60c12.grow__4c8a4"
        };

        public static SelPath userPanel = new SelPath
        {
            cssSelector = "#app-mount > div.appAsidePanelWrapper__5e6e2 > div.notAppAsidePanel__95814 > div.app_b1f720 > div > div.layers__1c917.layers_a23c37 > div > div > div > div > div.sidebar_e031be > section"
        };

        // name__4eb92 overflow__993fa
        public static SelPath channelName = new SelPath
        {
            cssSelector = "#channels > ul > li.containerDefault_ae2ea4.selected__11b62 > div > div > a > div > div.name__4eb92.overflow__993fa",
            tagClass = "name__4eb92 overflow__993fa"
        };

        public static SelPath channelId = new SelPath
        {
            cssSelector = "#channels > ul > li.containerDefault_ae2ea4.selected__11b62 > div > div > a",
            tagClass = "link_ddbb36",
            tagName = "a"
        };
    }

    public class SelPath
    {
        public string? cssWithIdStart;
        public string? cssWithIdEnd;
        public string? cssSelector;
        public string? xpath;
        public string? fullxpath;
        public string? tagClass;
        public string? tagName;
    }
}
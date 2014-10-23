// put user in viewModel once we have data to push out to main screen
var user = null;

$(window).resize(function () {
    ResizeDisplay();
});
$(window).load(function () {
    // try to load user from cookie
    ResizeDisplay();
});

SignInButtonClick = function () {
    var userName = $('#loginHeaderUserName').text();
    var password = $('#loginHeaderPassword').text();
    SignInUser(userName, password);
}
ResizeDisplay = function () {
    var headerBarRoot = $('#headerBarRoot');
    var mainScreen = $('#mainScreen');
    var mainBody = $('#mainBody');
    var sideBarRoot = $('#sidebarRoot');
    var openCloseSidebarDiv = $('#openCloseSidebarDiv');
    var sideBarRootWidth = sideBarRoot.outerWidth();

    $('#headerBarWrapper').outerWidth(headerBarRoot.innerWidth());
    mainScreen.outerHeight($(window).innerHeight() - headerBarRoot.outerHeight());
    mainBody.css('margin-left', sideBarRootWidth);
    mainBody.outerWidth(mainScreen.innerWidth() - sideBarRootWidth);
    openCloseSidebarDiv.css('padding-top', mainBody.innerHeight() / 2);
    openCloseSidebarDiv.height(mainBody.innerHeight() / 2);
}
ToggleSidebar = function () {
    var sidebarWrapper = $('#sidebarWrapper');
    var arrowSpan = $('#arrowSpan');

    if (sidebarWrapper.width() > 0) {
        sidebarWrapper.hide(0);
        sidebarWrapper.width(0);
        arrowSpan.text('>');
    } else {
        sidebarWrapper.show(0);
        sidebarWrapper.width(200);
        arrowSpan.text('<');
    }
    ResizeDisplay();
}
RedisplayHeader = function () {
    var userHeader = $('#userHeader');
    var loginHeader = $('#loginHeader');
    if (user === null) {
        userHeader.hide();
        loginHeader.show();
    } else {
        // put user data into header
        loginHeader.hide();
        userHeader.show();
    }
    ResizeDisplay();
}
SignInUser = function (userName, password) {
    var jsonData = JSON.stringify({ UserName: userName, Password: password });
    $.ajax({
        url: window.location.origin + '/Account/LogIn',
        data: jsonData,
        type: 'POST',
        contentType: 'application/json',
        success: function (data, textStatus, jqXHR) {
            if (!data.Success) {
                // display error message in login prompt
                return;
            }

            var appUser = data.Payload;
            user = new User(appUser.SessionID, appUser.UserName, appUser.Major);
            ko.utils.arrayForEach(appUser.Advisors, function (advisor) {
                user.advisors.push(new Advisor(advisor.Name, advisor.EMail));
            });

            // load user data into header bar
        },
        error: function () {
            alert('There is an issue with the server, please try again later');
        }
    });
}

User = function (sessionID, userName, major) {
    /* Properties */
    var self = this;
    self.userName = userName;
    self.major = major;
    self.sessionID = sessionID;
    self.advisors = ko.observableArray([]);

    /* Functions */
    self.signOut = function () {
        // make ajax request to account controller
        // if successful, set user = null and redisplay header bar
    }
}
Advisor = function (name, emailAddress) {
    /* Properties */
    var self = this;
    self.name = name;
    self.emailAddress = emailAddress;
}
var user = null;

$(window).resize(function () {
    ResizeDisplay();
});
$(window).load(function () {
    // try to load user from cookie

    ResizeDisplay();
});

ShowRegistrationDialog = function () {
    var userName = prompt('Enter desired user name');
    var waiting = true;
    var userNameAvailable = false;
    $.ajax({
        url: window.location.origin + '/Account/CheckUserName',
        data: JSON.stringify({ UserName: userName }),
        type: 'POST',
        contentType: 'application/json',
        success: function (data, textStatus, jqXHR) {
            if (!data.Success) {
                alert(data.Message);
                return;
            }

            if (!data.Payload) {
                alert('User name ' + userName + ' not available');
                return;
            }

            var password = prompt('Enter password');
            var major = prompt('Enter major');
            var registrationRequest = { UserName: userName, Password: password, Major: major };
            $.ajax({
                url: window.location.origin + '/Account/Register',
                data: JSON.stringify(registrationRequest),
                type: 'POST',
                contentType: 'application/json',
                success: function (data, textStatus, jqXHR) {
                    if (!data.Success) {
                        alert(data.Message);
                        return;
                    }

                    var appUser = data.Payload;
                    user = new User(appUser.SessionID, appUser.UserName, appUser.Major);
                    ko.utils.arrayForEach(appUser.Advisors, function (advisor) {
                        user.advisors.push(new Advisor(advisor.Name, advisor.EMail));
                    });

                    // set cookie

                    RedisplayHeader();
                }
            });
        },
        error: function () {
            alert('There is an issue with the server, please try again later');
            waiting = false;
        }
    });
    // while userName exists, ask for userName
    // get password
    // get major
}
SignInButtonClick = function () {
    var userName = $('#loginHeaderUserName').val();
    var password = $('#loginHeaderPassword').val();
    SignInUser(userName, password);
    $('#loginHeaderUserName').val('');
    $('#loginHeaderPassword').val('');
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
        sidebarWrapper.width(0);
        arrowSpan.text('>');
    } else {
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
        loginHeader.hide();
        userHeader.show();
        $('#userHeaderStudentName').text(user.userName);
        $('#userHeaderMajor').text(user.major);
    }
    ResizeDisplay();
}
SignInUser = function (userName, password) {
    var jsonData = { UserName: userName, Password: password };
    $.ajax({
        url: window.location.origin + '/Account/Login',
        data: JSON.stringify(jsonData),
        type: 'POST',
        contentType: 'application/json',
        success: function (data, textStatus, jqXHR) {
            if (!data.Success) {
                alert(data.Message);
                return;
            }

            var appUser = data.Payload;
            user = new User(appUser.SessionID, appUser.UserName, appUser.Major);
            ko.utils.arrayForEach(appUser.Advisors, function (advisor) {
                user.advisors.push(new Advisor(advisor.Name, advisor.EMail));
            });

            // set cookie

            RedisplayHeader();
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
        var jsonData = { UserName: self.userName };
        $.ajax({
            url: window.location.origin + '/Account/Logout',
            data: JSON.stringify(jsonData),
            type: 'POST',
            contentType: 'application/json'
        });
        user = null;

        RedisplayHeader();
    }
}
Advisor = function (name, emailAddress) {
    /* Properties */
    var self = this;
    self.name = name;
    self.emailAddress = emailAddress;
}
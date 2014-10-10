$(window).resize(function () {
    ResizeDisplay();
});
$(window).load(function () {
    $('#openCloseSidebarDiv').click(function () {
        ToggleSidebar();
    });

    ResizeDisplay();
});

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
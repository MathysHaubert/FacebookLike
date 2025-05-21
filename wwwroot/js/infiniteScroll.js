window.initInfiniteScroll = function (dotNetHelper) {
    window.onscroll = function () {
        if ((window.innerHeight + window.scrollY) >= document.body.offsetHeight - 100) {
            dotNetHelper.invokeMethodAsync('OnScrollToBottom');
        }
    };
}; 
window.blazoredToast = {
    show: function (message) {
        if (window.Blazored && window.Blazored.Toast) {
            window.Blazored.Toast.show(message);
        }
    }
};

window.handlePasteImage = function (dotNetRef) {
    document.addEventListener('paste', async function pasteHandler(e) {
        if (e.clipboardData && e.clipboardData.items) {
            for (let i = 0; i < e.clipboardData.items.length; i++) {
                let item = e.clipboardData.items[i];
                if (item.type.indexOf('image') !== -1) {
                    let file = item.getAsFile();
                    let reader = new FileReader();
                    reader.onload = function (event) {
                        // You should upload the image to your backend here and get a URL
                        // For demo, we just use the base64
                        dotNetRef.invokeMethodAsync('OnImagePasted', event.target.result);
                    };
                    reader.readAsDataURL(file);
                    e.preventDefault();
                    document.removeEventListener('paste', pasteHandler);
                    break;
                }
            }
        }
    });
};

window.scrollToBottom = function (element) {
    if (element && element.scrollHeight) {
        element.scrollTop = element.scrollHeight;
    }
};

window.registerConversationFocus = function(dotNetRef) {
    window.addEventListener('focus', function onFocus() {
        dotNetRef.invokeMethodAsync('OnPageFocus');
    });
};
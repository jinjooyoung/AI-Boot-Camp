mergeInto(LibraryManager.library, {
    CopyToClipboardWebGL: function (text) {
        var stringText = UTF8ToString(text);
        
        // 현대적인 브라우저 클립보드 API 사용
        if (navigator.clipboard && navigator.clipboard.writeText) {
            navigator.clipboard.writeText(stringText).then(function() {
                console.log("WebGL Clipboard: Copy success");
            }).catch(function(err) {
                console.error("WebGL Clipboard: Copy failed", err);
            });
        } else {
            // 구형 브라우저 우회책
            var textArea = document.createElement("textarea");
            textArea.value = stringText;
            document.body.appendChild(textArea);
            textArea.select();
            document.execCommand("copy");
            document.body.removeChild(textArea);
        }
    }
});
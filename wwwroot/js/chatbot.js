$(document).ready(function () {
    // --- Chat Widget Elements ---
    const chatInterface = $('#chat-interface');
    const chatToggleButton = $('#chat-toggle-button');
    const chatBox = $('#chatbox');
    const messageInput = $('#chat-message-input');
    const sendButton = $('#chat-send-button');
    const chatCloseButton = $('#chat-close-button');
    // -----------------------------------------------------------------------------------

    // --- Session ID Management ---
    let sessionId = sessionStorage.getItem('chatbotSessionId');
    if (!sessionId) {
        sessionId = generateGuid();
        sessionStorage.setItem('chatbotSessionId', sessionId);
    }
    console.log("Chat Session ID:", sessionId);

    // --- Trigger Welcome Intent on Load ---
    triggerWelcomeIntent();

    // --- Event Handlers ---
    chatToggleButton.click(function () {
        chatInterface.toggleClass('open');
        if (chatInterface.hasClass('open')) {
            scrollToBottom();
            messageInput.focus();
        }
    });

    sendButton.click(sendMessage);
    messageInput.keypress(function (e) {
        if (e.which === 13) {
            sendMessage();
            return false;
        }
    });
    chatCloseButton.click(function () {
        chatInterface.removeClass('open');
    });

    // --- Trigger Welcome Intent ---
    function triggerWelcomeIntent() {
        console.log("Triggering Welcome Intent for session:", sessionId);
        chatBox.html('');
        appendMessage('bot', 'Bot: Connecting...');
        let connectingMessage = chatBox.children('.message').last();

        const chatRequest = {
            eventName: "WELCOME",
            sessionId: sessionId
        };

        $.ajax({
            url: '/Chatbot/SendMessage',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(chatRequest),
            success: function (response) {
                if (connectingMessage) connectingMessage.remove();
                handleBotResponse(response);
            },
            error: function (xhr, status, error) {
                if (connectingMessage) connectingMessage.remove();
                console.error("Error triggering welcome intent:", status, error, xhr.responseText);
                try {
                    const errorResponse = JSON.parse(xhr.responseText);
                    if (errorResponse && errorResponse.replies && errorResponse.replies.length > 0) {
                        handleBotResponse(errorResponse);
                        return;
                    }
                } catch (e) { }
                appendMessage('bot', 'Bot: Oops! Could not connect.');
            }
        });
    }

    // --- Core Chat Logic ---
    function sendMessage() {
        const messageText = messageInput.val().trim();
        if (!messageText) {
            return;
        }

        appendMessage('user', messageText);
        messageInput.val('');

        const chatRequest = {
            message: messageText,
            sessionId: sessionId
        };

        appendMessage('bot', 'Bot: Thinking...');
        let thinkingMessage = chatBox.children('.message').last();

        $.ajax({
            url: '/Chatbot/SendMessage',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(chatRequest),
            success: function (response) {
                if (thinkingMessage) thinkingMessage.remove();
                handleBotResponse(response);
            },
            error: function (xhr, status, error) {
                if (thinkingMessage) thinkingMessage.remove();
                console.error("Error sending message:", status, error, xhr.responseText);
                try {
                    const errorResponse = JSON.parse(xhr.responseText);
                    if (errorResponse && errorResponse.replies && errorResponse.replies.length > 0) {
                        handleBotResponse(errorResponse);
                        return;
                    }
                } catch (e) { }
                appendMessage('bot', 'Bot: Oops! Error sending message.');
            },
            complete: function () {
                messageInput.focus();
            }
        });
    }

    // --- *** UPDATED: Handle both text replies AND rich content payload *** ---
    function handleBotResponse(response) {
        console.log("Full response:", response);

        // Handle text replies
        if (response && response.replies && Array.isArray(response.replies) && response.replies.length > 0) {
            response.replies.forEach(reply => {
                if (reply) {
                    appendMessage('bot', reply);
                }
            });
        }

        // Handle rich content payload
        if (response && response.payload) {
            console.log("Payload received:", response.payload);

            // Check if payload has richContent property
            if (response.payload.richContent && Array.isArray(response.payload.richContent)) {
                console.log("Found richContent:", response.payload.richContent);
                renderRichContent(response.payload.richContent);
            }
        }

        // Show error if neither replies nor payload exist
        if ((!response.replies || response.replies.length === 0) && !response.payload) {
            appendMessage('bot', 'Bot: Sorry, I didn\'t get a valid response.');
            console.log("Received invalid or empty response:", response);
        }

        scrollToBottom();
    }

    // --- Render Rich Content (Chips) ---
    function renderRichContent(richContentArray) {
        console.log("Rendering rich content:", richContentArray);

        richContentArray.forEach(function (section) {
            console.log("Processing section:", section);

            let infoTitle = '';
            let infoSubtitle = '';
            let chipOptions = [];

            if (Array.isArray(section)) {
                section.forEach(function (element) {
                    console.log("Processing element:", element);

                    if (element.type === 'info') {
                        infoTitle = element.title || '';
                        infoSubtitle = element.subtitle || '';
                    }
                    else if (element.type === 'chips' && element.options) {
                        chipOptions = element.options;
                    }
                });
            }

            if (infoTitle || infoSubtitle || chipOptions.length > 0) {
                renderCategoryList(infoTitle, infoSubtitle, chipOptions);
            }
        });

        scrollToBottom();
    }

    function renderCategoryList(title, subtitle, options) {
        let listHtml = '<div class="message bot">';
        listHtml += '<div class="category-list">';

        if (title) {
            listHtml += '<strong>' + escapeHtml(title) + '</strong>';
        }
        if (subtitle) {
            listHtml += '<div style="font-size: 0.9em; margin-bottom: 10px;">' + escapeHtml(subtitle) + '</div>';
        }

        options.forEach(function (option) {
            listHtml += '<button class="category-item" data-text="' + escapeHtml(option.text) + '">' +
                escapeHtml(option.text) +
                '</button>';
        });

        listHtml += '</div></div>';
        chatBox.append(listHtml);

        // Delegate click handler for category items
        chatBox.off('click', '.category-item').on('click', '.category-item', function () {
            const categoryText = $(this).data('text');

            // Convert category name to URL-friendly format (remove diacritics)
            const urlFriendlyName = removeDiacritics(categoryText);

            // Navigate to the category page
            window.location.href = '/phim-theo-the-loai/' + urlFriendlyName + '/trang-1';
        });
    }
    function removeDiacritics(str) {
        return str.normalize('NFD')
            .replace(/[\u0300-\u036f]/g, '')
            .replace(/đ/g, 'd')
            .replace(/Đ/g, 'D')
            .replace(/\s+/g, '-')
            .toLowerCase();
    }

    // Helper function to send message programmatically (for chip clicks)
    function sendMessageToBot(messageText) {
        const chatRequest = {
            message: messageText,
            sessionId: sessionId
        };

        appendMessage('bot', 'Bot: Thinking...');
        let thinkingMessage = chatBox.children('.message').last();

        $.ajax({
            url: '/Chatbot/SendMessage',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(chatRequest),
            success: function (response) {
                if (thinkingMessage) thinkingMessage.remove();
                handleBotResponse(response);
            },
            error: function (xhr, status, error) {
                if (thinkingMessage) thinkingMessage.remove();
                console.error("Error sending message:", status, error);
                appendMessage('bot', 'Bot: Oops! Error sending message.');
            }
        });
    }

    // --- Utility Functions ---
    function appendMessage(sender, text) {
        const messageClass = sender === 'user' ? 'user' : 'bot';
        const escapedText = escapeHtml(text);
        const messageDiv = `<div class="message ${messageClass}">${escapedText}</div>`;
        chatBox.append(messageDiv);
    }

    function scrollToBottom() {
        setTimeout(() => {
            chatBox.scrollTop(chatBox[0].scrollHeight);
        }, 0);
    }

    function generateGuid() {
        function s4() {
            return Math.floor((1 + Math.random()) * 0x10000).toString(16).substring(1);
        }
        return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
    }

    function escapeHtml(unsafe) {
        if (typeof unsafe !== 'string') {
            return unsafe;
        }
        return unsafe
            .replace(/&/g, "&amp;")
            .replace(/</g, "&lt;")
            .replace(/>/g, "&gt;")
            .replace(/"/g, "&quot;")
            .replace(/'/g, "&#039;");
    }
});
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
        sessionId = generateGuid(); // Use a simple GUID generator
        sessionStorage.setItem('chatbotSessionId', sessionId);
    }
    console.log("Chat Session ID:", sessionId);

    // --- *** NEW: Trigger Welcome Intent on Load *** ---
    triggerWelcomeIntent();
    // -----------------------------------------------

    // --- Event Handlers ---
    chatToggleButton.click(function () {
        chatInterface.toggleClass('open'); // Toggle visibility using the class
        // Optional: Re-trigger welcome only if chat is opened AND empty
        // if (chatInterface.hasClass('open') && chatBox.children('.message.bot').length <= 1) { // Check if only initial connecting message exists
        //    triggerWelcomeIntent();
        // }
        if (chatInterface.hasClass('open')) {
            scrollToBottom();
            messageInput.focus(); // Focus input when chat opens
        }
    });

    sendButton.click(sendMessage);
    messageInput.keypress(function (e) {
        if (e.which === 13) { // Enter key pressed
            sendMessage();
            return false; // Prevent default form submission behavior
        }
    });
    chatCloseButton.click(function () {
        chatInterface.removeClass('open'); // Simply hide the chat interface
    });
    // --- *** NEW: Function to Trigger Welcome Intent *** ---
    function triggerWelcomeIntent() {
        console.log("Triggering Welcome Intent for session:", sessionId);

        // Optional: Clear previous bot messages if desired when triggering welcome
        // chatBox.find('.message.bot').remove(); // Uncomment to clear history on welcome

        // Display connecting indicator (replaces the initial hardcoded message)
        chatBox.html(''); // Clear chatbox before showing connecting
        appendMessage('bot', 'Bot: Connecting...'); // Use appendMessage for consistency
        let connectingMessage = chatBox.children('.message').last();

        const chatRequest = {
            eventName: "WELCOME", // Send the event name
            sessionId: sessionId
        };

        $.ajax({
            url: '/Chatbot/SendMessage', // Adjust area if needed
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(chatRequest),
            // --- Headers for AntiForgeryToken (Uncomment if needed) ---
            // headers: {
            //     'RequestVerificationToken': antiForgeryToken
            // },
            // ------------------------------------------------------
            success: function (response) {
                if (connectingMessage) connectingMessage.remove(); // Remove connecting indicator
                // Use the new handler
                handleBotResponse(response);
            },
            error: function (xhr, status, error) {
                if (connectingMessage) connectingMessage.remove(); // Remove connecting indicator
                console.error("Error triggering welcome intent:", status, error, xhr.responseText);
                // Try to parse error response if backend sends one
                try {
                    const errorResponse = JSON.parse(xhr.responseText);
                    if (errorResponse && errorResponse.replies && errorResponse.replies.length > 0) {
                        handleBotResponse(errorResponse); // Display structured error
                        return;
                    }
                } catch (e) { /* Ignore parse error */ }
                // Fallback generic error
                appendMessage('bot', 'Bot: Oops! Could not connect.');
            }
        });
    }
    // ----------------------------------------------------


    // --- Core Chat Logic (Text Messages) ---
    function sendMessage() {
        const messageText = messageInput.val().trim();
        if (!messageText) {
            return; // Don't send empty messages
        }

        appendMessage('user', messageText); // Display user message immediately
        messageInput.val(''); // Clear input field

        // *** Send 'message' field for regular text ***
        const chatRequest = {
            message: messageText, // Send the text message
            sessionId: sessionId
        };

        // Optional: Show thinking indicator
        appendMessage('bot', 'Bot: Thinking...');
        let thinkingMessage = chatBox.children('.message').last();

        $.ajax({
            url: '/Chatbot/SendMessage', // Adjust area if needed
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(chatRequest),
            // --- Headers for AntiForgeryToken (Uncomment if needed) ---
            // headers: {
            //     'RequestVerificationToken': antiForgeryToken
            // },
            // ------------------------------------------------------
            success: function (response) {
                if (thinkingMessage) thinkingMessage.remove(); // Remove thinking indicator
                // *** Use the new handler ***
                handleBotResponse(response);
                // ***************************
            },
            error: function (xhr, status, error) {
                if (thinkingMessage) thinkingMessage.remove(); // Remove thinking indicator
                console.error("Error sending message:", status, error, xhr.responseText);
                // Try to parse error response if backend sends one
                try {
                    const errorResponse = JSON.parse(xhr.responseText);
                    if (errorResponse && errorResponse.replies && errorResponse.replies.length > 0) {
                        handleBotResponse(errorResponse); // Display structured error
                        return;
                    }
                } catch (e) { /* Ignore parse error */ }
                // Fallback generic error
                appendMessage('bot', 'Bot: Oops! Error sending message.');
            },
            complete: function () {
                messageInput.focus(); // Keep focus on input
            }
        });
    }

    // --- *** NEW: Centralized function to handle bot responses *** ---
    function handleBotResponse(response) {
        if (response && response.replies && Array.isArray(response.replies) && response.replies.length > 0) {
            // Loop through the replies array and append each message
            response.replies.forEach(reply => {
                if (reply) { // Check if reply string is not null/empty
                    appendMessage('bot',  reply);
                }
            });
        } else {
            // Handle cases where response is invalid or replies array is empty
            appendMessage('bot', 'Bot: Sorry, I didn\'t get a valid response.');
            console.log("Received invalid or empty response:", response); // Log for debugging
        }
        scrollToBottom(); // Scroll after processing all replies
    }
    // --------------------------------------------------------------


    // --- Utility Functions (Keep as they were) ---
    function appendMessage(sender, text) {
        const messageClass = sender === 'user' ? 'user' : 'bot';
        // Basic HTML escaping
        const escapedText = $('<div>').text(text).html(); // Use jQuery's text() to escape HTML
        const messageDiv = `<div class="message ${messageClass}">${escapedText}</div>`;
        chatBox.append(messageDiv);
        // scrollToBottom(); // Moved scrolling to after handling all replies in handleBotResponse
    }

    function scrollToBottom() {
        // Use setTimeout to ensure DOM has updated after appending messages
        setTimeout(() => {
            chatBox.scrollTop(chatBox[0].scrollHeight);
        }, 0);
    }

    // Simple GUID generator
    function generateGuid() {
        function s4() { return Math.floor((1 + Math.random()) * 0x10000).toString(16).substring(1); }
        return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
    }

    // Optional: Pre-scroll if chat is already open on page load (might be redundant now welcome intent clears)
    // if (chatInterface.hasClass('open')) {
    //     scrollToBottom();
    // }

});
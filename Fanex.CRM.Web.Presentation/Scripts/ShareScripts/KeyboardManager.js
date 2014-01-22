

var KeyboardManager = function () {
    var self = this;
    self.Enums = {
        KeyCode: {
            Backspace: 8,
            Tab: 9,
            Enter: 13,
            Ctrl: 17,
            Alt: 18,
            LeftArrow: 37,
            UpArrow: 38,
            RightArrow: 39,
            DownArrow: 40,
            Number_0: 48,
            Number_9: 57,
            Character_a: 65,
            Character_z: 90,

            // Character ,
            Character_Comma: 188,
            // Character .
            Character_Period: 190
        }
    };
};

var keyboardManager = new KeyboardManager();
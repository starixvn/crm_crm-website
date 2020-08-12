export const handleNewResendOptions = (data, done) => {
    let dataList = data.map(item => {
        return {
            notificationId: item.notificationId,
            messageId: item.messageId,
            status: item.statusString,
            isResent: false
        }
    });

    done({
        isCheckedAll: false,
        dataList
    });
};

const functions = {
    handleNewResendOptions
}

export default functions;
import Site from '@jsroot/common/site';
import DataService from '@jsroot/common/dataService';

const recheckin = async function () {
    const url = Site.resolveClientUrl('Auth/CheckUserOnlineStatus');
    await DataService.postDataAsync(url).then(() => setTimeout(recheckin, 60000));
};

const isHost = document.getElementById('IsHost').checked;
if (isHost) {
    setTimeout(recheckin, 60000);
}
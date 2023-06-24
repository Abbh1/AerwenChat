import { defineStore,storeToRefs} from 'pinia'

const useSwitchStore = defineStore('switch', {
    state: () => {
        return {
            isPersonal: false,
            isFriends: false,
            isFriendIntro: false,
            FriendIntroData: null,
            isChat: false,
            isGroupChat: false,
            notOnlineChat: false,
        }
    },
    actions: {
        
    },
    getters: {

    }
})

export default useSwitchStore;
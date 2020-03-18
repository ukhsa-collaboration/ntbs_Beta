import Vue from "vue";

const timerTemplate = `
    <div class="dialog-box">
        <section>
            <h3 class="dialog-box-message">{{ message }}</h3>
            <h5>You will be automatically logged out of NTBS in {{ Math.floor(timer/60) }}m {{timer%60}}s due to inactivity.</h5>
            <div class="dialog-box-button-container">
                <button class="nhsuk-button nhsuk-button--secondary dialog-box-button" type="button" @click="rejectHandler">Kepp me logged in</button>
                <button class="nhsuk-button nhsuk-button--secondary dialog-box-button" type="button" @click="resolveHandler">Logout</button>
            </div>
        </section>
    </div>
    `;

export default Vue.extend({
    name: 'InactivityLeaveComponent',
    template: timerTemplate,
    props: {
        message: {
            type: String,
        },
        resolve: {
            type: Function,
        },
        reject: {
            type: Function,
        },
    },
    data: function () {
        return {
            timer: 120,
            interval: null
        }
    },
    mounted() {
        this.interval = setInterval(this.countDown, 1000);
    },
    methods: {
        resolveHandler() {
            // Resolve promise within vue-accessible-modal
            // In this implementation triggers .Confirm().then() block in FormLeaveChecker
            this.resolve();
        },
        rejectHandler() {
            // Reject promise within vue-accessible-modal
            // In this implementation triggers .Confirm().catch() block in FormLeaveChecker 
            this.reject();
        },
        countDown() {
            if (this.timer > 0) {
                this.timer--;
            } else {
                clearInterval(this.interval);
                this.resolveHandler();
            }
        },
    }
});


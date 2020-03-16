import Vue from "vue";

const timerTemplate = `
    <div class="dialog-box">
        <section>
            <h3 class="dialog-box-message">{{ message }}</h3>
            <h5>You will be automatically logged out of NTBS in {{ Math.floor(timer/60) }}m {{timer%60}}s due to inactivity.</h5>
            <div class="dialog-box-button-container">
                <button class="nhsuk-button nhsuk-button--secondary dialog-box-button" type="button" @click="rejectHandler">Stay</button>
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
            counter: false,
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
            var n = this.timer;
            if (!this.counter) {
                this.counter = true;
            } else if (n > 0) {
                n = n - 1;
                this.timer = n;
            } else {
                clearInterval(this.interval);
                this.counter = false;
                this.resolveHandler();
            }
        },
    }
});


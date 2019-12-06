import Vue from "vue";

const HideManualTestResults = Vue.extend({
    methods: {
        hideOrShowResults: function (value: any) {
            if (this.isTestCarriedOutFalse()) {
                this.$refs["manual-test-results"].classList.add("hidden");
            } else {
                this.$refs["manual-test-results"].classList.remove("hidden");
            }
        },
        isTestCarriedOutFalse: function () {
            return this.$refs["inner-validate"].$refs["test-carried-out-no"].checked;
        }
    }
});

export default HideManualTestResults;

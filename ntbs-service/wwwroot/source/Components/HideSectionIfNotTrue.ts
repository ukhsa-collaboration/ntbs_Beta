import Vue from "vue";

const HideSectionIfNotTrue = Vue.extend({
    methods: {
        hideOrShowSection: function (value: any) {
            if (this.shouldHide()) {
                this.$refs["conditional-section"].classList.add("hidden");
            } else {
                this.$refs["conditional-section"].classList.remove("hidden");
            }
        },
        shouldHide: function () {
            let trueInput = this.$refs["inner-validate"].$refs["conditional-true"];
            return !trueInput.checked;
        }
    }
});

export default HideSectionIfNotTrue;

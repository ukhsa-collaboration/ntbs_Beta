import Vue from "vue";

const PrintButton = Vue.extend({
    mounted:  function () {
        this.$el.classList.remove("hidden");
    },
    methods: {
        print: function () {
            window.print();
        }
    }
});

export default PrintButton;
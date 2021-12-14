import Vue from "vue";

const NavigationWithSubmenu = Vue.extend({
    data: function () {
        return {
            showMenu: false
        }
    },
    created(): void {
        window.addEventListener("click", this.hideNavDropdown.bind(this));
    },
    methods: {
        hideNavDropdown: function (event: MouseEvent )
        {
            let inputElement = (<HTMLInputElement>event.target);
            let headerLink = this.$el.getElementsByClassName('nav-with-submenu-header-link')[0];
            if (inputElement != headerLink && !headerLink.contains(inputElement)) {
                this.showMenu = false;
            }
        },
        toggleMenu: function (event: MouseEvent) {
            event.preventDefault();
            this.showMenu = !this.showMenu;
        }
    }
});

export default NavigationWithSubmenu;

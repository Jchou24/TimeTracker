<template>
    <div class="two-column">
        <div class="fixed-sidebar">
            <div class="fixed-sidebar-background" />
            <div class="fixed-sidebar-content">
                <slot name="left"></slot>
            </div>
        </div>
        <div class="content">
            <slot name="right"></slot>
        </div>
    </div>
</template>

<style lang="scss" scoped>
    .two-column{
        display: flex;
        flex-direction: row;
        justify-content: center;
        width: 100%;
    }

    $width-sidebar: 350px;

    .two-column .fixed-sidebar{
        position: fixed;
        left: 0px;
        top: 0px;
        width: $width-sidebar;
        height: 100vh;
        display: flex;
        justify-content: center;
        z-index: 1;

        .fixed-sidebar-content{
            display: flex;
            flex-direction: column;
            padding-top: 100px;
            padding-bottom: 20px;
            overflow: auto;

            &::-webkit-scrollbar
            {
                width: 0px;
                height: 0px;
            }
        }

        .fixed-sidebar-background{
            position: absolute;
            background: $img-dark-wood;
            // filter: blur(2px);
            // filter: brightness(80%);
            filter: brightness(100%) opacity(90%);

            width: 100%;
            height: 100%;
        }
    }

    .two-column .content{
        padding-left: $width-sidebar;
        z-index: 0;
    }
</style>
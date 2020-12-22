// import { computed } from '@vue/composition-api'
// import { useWindowSize } from 'vue-use-web'

enum ScreenSize{
    xl,
    lg,
    md,
    sm,
    xs,
}

function GetSize(size: number) {

    if( 1904 <= window.innerWidth ){
        return ScreenSize.xl
    }else if( 1264 <= window.innerWidth && window.innerWidth < 1904 ){
        return ScreenSize.lg
    }else if( 960 <= window.innerWidth && window.innerWidth < 1264 ){
        return ScreenSize.md
    }else if( 600 <= window.innerWidth && window.innerWidth < 960 ){
        return ScreenSize.sm
    }else{
        return ScreenSize.xs
    }    
}

// const GetScreenSize = () => computed( () => {
//     const { width, height } = useWindowSize({
//         throttleMs: 100
//     });

//     return GetSize(width.value)    
// })

export {
    ScreenSize,
    GetSize,
    // GetScreenSize,
}
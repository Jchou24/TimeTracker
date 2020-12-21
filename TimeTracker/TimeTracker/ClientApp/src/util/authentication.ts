// function GetGuid() {
//     return ([1e7]+-1e3+-4e3+-8e3+-1e11).replace(/[018]/g, c =>
//       (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
//     );
// }

import { IClaims } from "@/models/authentication";

function GetGuid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
        const r = Math.random() * 16 | 0,
            v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}

const GetGuids = ( users: Array<IClaims>) => users.map( selectedUser => selectedUser.guid )

const IsEqualUsers = ( left: Array<string>, right: Array<string> ) => {
    const a = new Set(left)
    const b = new Set(right)
    const intersection = new Set([ ...a].filter( x => b.has(x) ))
    return a.size == intersection.size && b.size == intersection.size
}

export {
    GetGuid,
    GetGuids,
    IsEqualUsers,
}
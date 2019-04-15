const levels = {
  firstRoomA: [
    '<____><____>0',
    '/    |/    |0',
    '/    ()    |0',
    '/          |0',
    '[----------]0'
  ]
};

export class Layouts {

  /**
   * @public
   * @param  {string} key unique identifier of the needed layout
   * @return {Array<string>} rows of data contained in strings
   */
  static byName(key) {
    return levels[key];
  }
}

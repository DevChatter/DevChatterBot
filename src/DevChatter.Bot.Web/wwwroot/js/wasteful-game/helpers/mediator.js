export class Event {
  /**
   * @public
   * @param {object} args
   */
  constructor(args) {
    this._process = true;
    this._args = args || {};
  }

  /**
   * @public
   * @returns {boolean}
   */
  get process() {
    return this._process;
  }

  /**
   * @public
   * @param {boolean} process
   */
  set process(process) {
    this._process = process;
  }

  /**
   * @public
   * @returns {object}
   */
  get args() {
    return this._args;
  }

  /**
   * @public
   * @param {object} args
   */
  set args(args) {
    this._args = args;
  }
}

class Mediator {
  constructor() {
    this.channels = {};
  }

  /**
   * @public
   * @param {symbol|string} channel
   * @param {function} fn
   */
  subscribe(channel, fn) {
    if (!this.channels[channel]) {
      this.channels[channel] = [];
    }

    this.channels[channel].push({
      context: this,
      callback: fn
    });
  }

  /**
   * @public
   * @param {symbol|string} channel
   * @param {function} fn
   */
  unsubscribe(channel, fn) {
    if (!this.channels[channel]) {
      return;
    }

    const index = this.channels[channel].indexOf(fn);
    if(index !== -1) {
      this.channels[channel].splice(index, 1);
    }
  }

  /**
   * @public
   * @param {symbol|string} channel
   * @param {Event} args
   * @returns {Event}
   */
  publish(channel, args) {
    let retVal = new Event(args);
    if (!this.channels[channel]) {
      return retVal;
    }

    this.channels[channel].forEach(function(subscription) {
      Object.assign(retVal, subscription.callback.apply(subscription.context, [retVal]));
      if(!retVal.process) {
        return retVal;
      }
    });

    return retVal;
  }

  /**
   * @public
   * @param {object} obj
   */
  installTo(obj) {
    obj.channels = {};
    obj.publish = this.publish;
    obj.subscribe = this.subscribe;
    obj.unsubscribe = this.unsubscribe;
  }
}

const mediator = new Mediator();
export default mediator;

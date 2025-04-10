import {Injectable} from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class MyCacheService {
  private cache = new Map<string, any>();
  private expirationTimes = new Map<string, number>();

  constructor() {
  }

  set(key: string, value: any, ttl: number = 30 * 1000): void { // TTL u milisekundama (default 30 sek)
    this.cache.set(key, value);
    this.expirationTimes.set(key, Date.now() + ttl);
  }

  get<T>(key: string): T | null {
    const expiration = this.expirationTimes.get(key);

    if (expiration && expiration < Date.now()) {
      this.cache.delete(key);
      this.expirationTimes.delete(key);
      return null;
    }

    return this.cache.get(key) || null;
  }

  has(key: string): boolean {
    return this.cache.has(key) && (!this.expirationTimes.get(key) || this.expirationTimes.get(key)! > Date.now());
  }

  clear(): void {
    this.cache.clear();
    this.expirationTimes.clear();
  }
}
